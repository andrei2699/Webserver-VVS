using System.IO;

namespace Webserver.IO
{
    public class FilePathValidator : IFilePathValidator
    {
        public bool Validate(string path)
        {
            return File.Exists(path);
        }
    }
}
