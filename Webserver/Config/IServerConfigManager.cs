namespace Webserver.Config
{
    public interface IServerConfigManager
    {
        ServerConfig ReadConfig();

        void WriteConfig(ServerConfig config);
    }
}
