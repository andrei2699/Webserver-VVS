using System;
using System.Collections.Generic;
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
        private const int ThreadCount = 5;

        private readonly IFilePathProvider _filePathProvider;
        private readonly IServerConfigManager _serverConfigManager;
        private readonly IRequestParser _requestParser;
        private readonly IResponseCreator _responseCreator;
        private readonly IFilePathValidator _filePathValidator;

        private TcpListener _listener;
        private ServerConfig _serverConfig;
        private List<Thread> _threads = new();

        private const string MaintenanceDefaultFilePath = "Maintenance.html";

        public Server(IFilePathProvider filePathProvider, IServerConfigManager serverConfigManager,
            IRequestParser requestParser, IResponseCreator responseCreator, IFilePathValidator filePathValidator)
        {
            _filePathProvider = filePathProvider;
            _serverConfigManager = serverConfigManager;
            _requestParser = requestParser;
            _responseCreator = responseCreator;
            _filePathValidator = filePathValidator;
        }

        public void Start()
        {
            _serverConfig = _serverConfigManager.ReadConfig();

            _serverConfigManager.WriteConfig(new ServerConfig(_serverConfig.Port, _serverConfig.FilePath,
                _serverConfig.MaintenanceFilePath,
                ServerState.Running));

            if (_serverConfig.State is not ServerState.Maintenance)
            {
                _serverConfig.State = ServerState.Running;
            }

            _filePathProvider.SetRootPath(_serverConfig.FilePath);

            _listener = new TcpListener(IPAddress.Any, _serverConfig.Port);
            _listener.Start();
            Console.WriteLine($"Web Server Running on {_serverConfig.Port.ToString()}...");

            _threads = new List<Thread>();
            for (var i = 0; i < ThreadCount; i++)
            {
                var thread = new Thread(StartListen);
                _threads.Add(thread);

                thread.Start();
            }

            foreach (var thread in _threads)
            {
                thread.Join();
            }

            if (_serverConfig.State is ServerState.Running)
            {
                _serverConfig.State = ServerState.Stopped;
            }

            Console.WriteLine("Web Server Stopping...");

            _serverConfigManager.WriteConfig(new ServerConfig(_serverConfig.Port, _serverConfig.FilePath,
                _serverConfig.MaintenanceFilePath,
                _serverConfig.State));
        }

        public void OnStatusChanged(ServerState serverState)
        {
            _serverConfig.State = serverState;
            _serverConfigManager.WriteConfig(_serverConfig);

            if (serverState == ServerState.Stopped)
            {
                foreach (var thread in _threads)
                {
                    thread.Interrupt();
                }

                Environment.Exit(0);
            }
        }

        public void OnFilePathChanged(string filePath)
        {
            switch (_serverConfig.State)
            {
                case ServerState.Running:
                {
                    _serverConfig.FilePath = filePath;
                    _serverConfigManager.WriteConfig(_serverConfig);
                    _filePathProvider.SetRootPath(filePath);
                }
                    break;
                case ServerState.Maintenance:
                {
                    _serverConfig.MaintenanceFilePath = filePath;
                    _serverConfigManager.WriteConfig(_serverConfig);
                }
                    break;
            }
        }

        private void StartListen()
        {
            while (_serverConfig.State != ServerState.Stopped)
            {
                try
                {
                    var socket = _listener.AcceptSocket();

                    var receivedBytes = new byte[1024];
                    socket.Receive(receivedBytes, receivedBytes.Length, 0);

                    try
                    {
                        var requestData = _requestParser.Parse(receivedBytes);

                        if (_serverConfig.State == ServerState.Maintenance && requestData.Target != "/config")
                        {
                            var maintenanceFilePath = _serverConfig.MaintenanceFilePath;
                            if (!_filePathValidator.Validate(maintenanceFilePath))
                            {
                                maintenanceFilePath = MaintenanceDefaultFilePath;
                            }

                            requestData = new RequestData(requestData.Method, maintenanceFilePath,
                                requestData.Version, requestData.Headers, requestData.Body);
                        }

                        socket.Send(_responseCreator.Create(requestData));
                    }
                    catch (ServerException serverException)
                    {
                        socket.Send(
                            _responseCreator.Create(new ResponseStatusLine("HTTP/1.1", serverException.StatusCode)));
                    }

                    socket.Close();
                }
                catch (ThreadInterruptedException)
                {
                    if (_serverConfig.State == ServerState.Stopped)
                    {
                        return;
                    }
                }
            }
        }
    }
}
