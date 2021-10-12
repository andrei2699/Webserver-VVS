using System.IO;

namespace Webserver.IO
{
    public class FileReader : IFileReader
    {
        public byte[] Read(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}
