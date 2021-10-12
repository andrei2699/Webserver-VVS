using System.IO;

namespace Webserver.IO
{
    public class FileWriter : IFileWriter
    {
        public void Write(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}
