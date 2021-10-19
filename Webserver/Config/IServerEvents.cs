namespace Webserver.Config
{
    public interface IServerEvents
    {
        void ChangeFilePath(string filePath);

        void ChangeState(ServerState serverState);
    }
}
