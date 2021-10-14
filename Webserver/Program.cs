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
            new Server(
                    new ServerConfigManager(fileReader, new FileWriter()),
                    new RequestParser(),
                    new ResponseCreator(new ResponseHeaderParser(), new FilePathProvider(), fileReader,
                        new ContentTypeHeaderProvider()))
                .Start();
        }
    }
}
