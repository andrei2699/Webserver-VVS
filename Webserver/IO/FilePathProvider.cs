namespace Webserver.IO
{
    public class FilePathProvider : IFilePathProvider
    {
        public string Provide(string path)
        {
            return path;
        }
    }
}
