using System;

namespace Webserver.Config
{
    public class ServerEvents : IServerEvents
    {
        public event Action<ServerState> OnStatusChanged;

        public event Action<string> OnFilePathChanged;

        public void ChangeFilePath(string filePath)
        {
            OnFilePathChanged?.Invoke(filePath);
        }

        public void ChangeState(ServerState serverState)
        {
            OnStatusChanged?.Invoke(serverState);
        }
    }
}
