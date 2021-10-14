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
            var filePathProvider = new FilePathProvider(new FilePathValidator());
            new Server(
                    filePathProvider,
                    new ServerConfigManager(fileReader, new FileWriter()),
                    new RequestParser(),
                    new ResponseCreator(new ResponseHeaderParser(), filePathProvider, fileReader,
                        new ContentTypeHeaderProvider()))
                .Start();
        }
    }
}
