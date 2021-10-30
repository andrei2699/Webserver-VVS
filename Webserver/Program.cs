using Webserver.Config;
using Webserver.HTTPHeaders;
using Webserver.IO;
using Webserver.Request;
using Webserver.Response;

namespace Webserver
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileReader = new FileReader();
            var filePathValidator = new FilePathValidator();
            var filePathProvider = new FilePathProvider(filePathValidator);
            var serverManager = new ServerEvents();
            var server = new Server(
                filePathProvider,
                new ServerConfigManager(fileReader, new FileWriter()),
                new RequestParser(),
                new ResponseCreator(new ResponseHeaderParser(), filePathProvider, fileReader,
                    new ContentTypeHeaderProvider(), new ConfigRequestHandler(serverManager)),
                filePathValidator);

            serverManager.OnStatusChanged += server.OnStatusChanged;
            serverManager.OnFilePathChanged += server.OnFilePathChanged;

            server.Start();
        }
    }
}
