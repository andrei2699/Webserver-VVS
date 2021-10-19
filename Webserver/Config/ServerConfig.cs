namespace Webserver.Config
{
    public record ServerConfig(int Port)
    {
        public string FilePath { get; set; }

        public string MaintenanceFilePath { get; set; }

        public ServerState State { get; set; }

        public ServerConfig(int port, string filePath, string maintenanceFilePath, ServerState state) : this(port)
        {
            FilePath = filePath;
            MaintenanceFilePath = maintenanceFilePath;
            State = state;
        }

        public ServerConfig() : this(8080)
        {
        }

        public void Deconstruct(out int port, out string filepath, out string maintenanceFilePath,
            out ServerState state)
        {
            port = Port;
            filepath = FilePath;
            maintenanceFilePath = MaintenanceFilePath;
            state = State;
        }
    }
}
