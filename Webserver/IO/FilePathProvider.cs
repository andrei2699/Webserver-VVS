using System;
using System.IO;
using System.Threading;
using System.Web;

namespace Webserver.IO
{
    public class FilePathProvider : IFilePathProvider
    {
        private readonly IFilePathValidator _filePathValidator;

        private const string DefaultPagesFolderName = "DefaultPages";

        private readonly Semaphore _semaphore = new(1, 1);
        private string _rootPath;

        public FilePathProvider(IFilePathValidator filePathValidator)
        {
            _filePathValidator = filePathValidator;
        }

        public void SetRootPath(string rootPath)
        {
            _semaphore.WaitOne();
            _rootPath = rootPath;

            if (_rootPath.EndsWith('/') || _rootPath.EndsWith('\\'))
            {
                _rootPath = _rootPath[..^1];
            }

            _semaphore.Release();
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
                return IsDefaultPage(path)
                    ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultPagesFolderName, path)
                    : path;
            }

            var finalPath = Path.Combine(_rootPath, path);
            finalPath = HttpUtility.UrlDecode(finalPath);

            if (IsDefaultPage(path) && !_filePathValidator.Validate(finalPath))
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultPagesFolderName, path);
            }

            return finalPath;
        }

        private static bool IsDefaultPage(string path)
        {
            return path is "400.html" or "404.html" or "405.html" or "500.html" or "Maintenance.html";
        }
    }
}
