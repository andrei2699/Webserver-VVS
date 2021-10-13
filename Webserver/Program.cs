using Webserver.Config;
using Webserver.IO;
using Webserver.Request;
using Webserver.Response;

namespace Webserver
{
    class Program
    {
        static void Main(string[] args)
        {
            new Server(new ServerConfigManager(new FileReader(), new FileWriter()), new RequestParser(), new ResponseCreator()).Start();
        }
    }
}
