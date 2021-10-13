﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Webserver.Config;
using Webserver.Exceptions;
using Webserver.Request;
using Webserver.Response;

namespace Webserver
{
    public class Server
    {
        private readonly IServerConfigManager _serverConfigManager;
        private readonly IRequestParser _requestParser;
        private readonly IResponseCreator _responseCreator;

        private TcpListener _listener;

        public Server(IServerConfigManager serverConfigManager, IRequestParser requestParser,
            IResponseCreator responseCreator)
        {
            _serverConfigManager = serverConfigManager;
            _requestParser = requestParser;
            _responseCreator = responseCreator;
        }

        public void Start()
        {
            var serverConfig = _serverConfigManager.ReadConfig();
            _listener = new TcpListener(serverConfig.Port);
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

                byte[] headerBytes;
                try
                {
                    var requestData = _requestParser.Parse(receivedBytes);
                    headerBytes = _responseCreator.Create(new ResponseDataHeader(requestData.Version, HttpStatusCode.OK,
                        new Dictionary<string, string>()));
                }
                catch (ServerException serverException)
                {
                    headerBytes = _responseCreator.Create(new ResponseDataHeader("HTTP/1.1", serverException.StatusCode,
                        new Dictionary<string, string>()));
                }

                socket.Send(headerBytes);

                socket.Close();
            }
        }
    }
}
