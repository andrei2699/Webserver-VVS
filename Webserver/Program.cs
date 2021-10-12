using Webserver.Config;
using Webserver.IO;

namespace Webserver
{
    class Program
    {
        static void Main(string[] args)
        {
            new Server(new ServerConfigManager(new FileReader(), new FileWriter())).Start();
        }
    }
}
