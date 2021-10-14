using System;
using System.Net.Sockets;
using System.Threading;
using Webserver.Config;
using Webserver.Exceptions;
using Webserver.IO;
using Webserver.Request;
using Webserver.Response;

namespace Webserver
{
    public class Server
    {
        private readonly FilePathProvider _filePathProvider;
        private readonly IServerConfigManager _serverConfigManager;
        private readonly IRequestParser _requestParser;
        private readonly IResponseCreator _responseCreator;

        private TcpListener _listener;

        public Server(FilePathProvider filePathProvider, IServerConfigManager serverConfigManager,
            IRequestParser requestParser,
            IResponseCreator responseCreator)
        {
            _filePathProvider = filePathProvider;
            _serverConfigManager = serverConfigManager;
            _requestParser = requestParser;
            _responseCreator = responseCreator;
        }

        public void Start()
        {
            var (port, filePath) = _serverConfigManager.ReadConfig();
            _filePathProvider.SetRootPath(filePath);

            _listener = new TcpListener(port);
            _listener.Start();
            Console.Write("Web Server Running...");
            var thread = new Thread(StartListen);
            thread.Start();
        }

        private void StartListen()
        {
            while (true)
            {
                var socket = _listener.AcceptSocket();

                var receivedBytes = new byte[1024];
                socket.Receive(receivedBytes, receivedBytes.Length, 0);

                try
                {
                    var requestData = _requestParser.Parse(receivedBytes);
                    socket.Send(_responseCreator.Create(requestData));
                }
                catch (ServerException serverException)
                {
                    socket.Send(
                        _responseCreator.Create(new ResponseStatusLine("HTTP/1.1", serverException.StatusCode)));
                }

                socket.Close();
            }
        }
    }
}
