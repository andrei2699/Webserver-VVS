using System;
using System.Text;
using System.Text.Json;
using Webserver.IO;

namespace Webserver.Config
{
    public class ServerConfigManager : IServerConfigManager
    {
        private const string ConfigFileName = "config.json";
        private const string DefaultFolderPath = "public_html";

        private readonly IFileReader _fileReader;
        private readonly IFileWriter _fileWriter;

        public ServerConfigManager(IFileReader fileReader, IFileWriter fileWriter)
        {
            _fileReader = fileReader;
            _fileWriter = fileWriter;
        }

        public ServerConfig ReadConfig()
        {
            try
            {
                var configFileBytes = _fileReader.Read(ConfigFileName);
                var configFileContent = Encoding.ASCII.GetString(configFileBytes);
                return JsonSerializer.Deserialize<ServerConfig>(configFileContent);
            }
            catch
            {
                var config = new ServerConfig(8080, DefaultFolderPath, DefaultFolderPath, ServerState.Running);
                WriteConfig(config);
                return config;
            }
        }

        public void WriteConfig(ServerConfig config)
        {
            if (config == null)
            {
                return;
            }

            var serializedString = JsonSerializer.Serialize(config);
            _fileWriter.Write(ConfigFileName, serializedString);
        }
    }
}
