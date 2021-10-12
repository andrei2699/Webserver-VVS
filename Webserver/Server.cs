using System;
using System.Net.Sockets;
using System.Threading;
using Webserver.Config;

namespace Webserver
{
    public class Server
    {
        private readonly IServerConfigManager _serverConfigManager;
        private TcpListener _listener;

        public Server(IServerConfigManager serverConfigManager)
        {
            _serverConfigManager = serverConfigManager;
        }

        public void Start()
        {
            var serverConfig = _serverConfigManager.ReadConfig();
            _listener = new TcpListener(serverConfig.Port);
            _listener.Start();
            Console.Write("Web Server Running...");
            var thread = new Thread(new ThreadStart(StartListen));
            thread.Start();
        }

        private void StartListen()
        {
            while (true)
            {
                var socket = _listener.AcceptSocket();
            }
        }
    }
}
