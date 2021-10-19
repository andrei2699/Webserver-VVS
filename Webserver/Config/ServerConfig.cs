namespace Webserver.Config
{
    public record ServerConfig(int Port, string FilePath, string MaintenanceFilePath, ServerState State);
}
