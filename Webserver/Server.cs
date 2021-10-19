using System;
using System.Net;
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
            var (port, filePath, maintenanceFilePath, serverState) = _serverConfigManager.ReadConfig();
            _serverConfigManager.WriteConfig(new ServerConfig(port, filePath, maintenanceFilePath,
                ServerState.Running));

            if (serverState is ServerState.Stopped)
            {
                serverState = ServerState.Running;
                _filePathProvider.SetRootPath(filePath);
            }
            else
            {
                _filePathProvider.SetRootPath(maintenanceFilePath);
            }

            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();
            Console.WriteLine("Web Server Running...");
            var thread = new Thread(StartListen);
            thread.Start();

            thread.Join();

            if (serverState is ServerState.Running)
            {
                serverState = ServerState.Stopped;
            }

            Console.WriteLine("Web Server Stopping...");
            _serverConfigManager.WriteConfig(new ServerConfig(port, filePath, maintenanceFilePath,
                serverState));
        }

        public void OnStatusChanged(ServerState serverState)
        {
        }

        public void OnFilePathChanged(string filePath)
        {
            
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
