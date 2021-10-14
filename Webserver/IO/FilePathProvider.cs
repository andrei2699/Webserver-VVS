using System;
using System.IO;
using System.Web;

namespace Webserver.IO
{
    public class FilePathProvider : IFilePathProvider
    {
        private readonly IFilePathValidator _filePathValidator;

        private const string DefaultPagesFolderName = "DefaultPages";

        private string _rootPath;

        public FilePathProvider(IFilePathValidator filePathValidator)
        {
            _filePathValidator = filePathValidator;
        }

        public void SetRootPath(string rootPath)
        {
            _rootPath = rootPath;

            if (_rootPath.EndsWith('/') || _rootPath.EndsWith('\\'))
            {
                _rootPath = _rootPath[..^1];
            }
        }

        public string Provide(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return "";
            }

            if (path == "/")
            {
                path = "index.html";
            }

            if (path.StartsWith('/') || path.StartsWith('\\'))
            {
                path = path[1..];
            }

            if (string.IsNullOrEmpty(_rootPath))
            {
                if (path is "400.html" or "404.html" or "405.html" or "500.html")
                {
                    return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultPagesFolderName, path);
                }

                return path;
            }

            var finalPath = Path.Combine(_rootPath, path);
            finalPath = HttpUtility.UrlDecode(finalPath);

            if (path is "400.html" or "404.html" or "405.html" or "500.html")
            {
                if (!_filePathValidator.Validate(finalPath))
                {
                    return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultPagesFolderName, path);
                }
            }

            return finalPath;
        }
    }
}
