namespace Webserver.IO
{
    public interface IFilePathProvider
    {
        void SetRootPath(string rootPath);
        
        string Provide(string path);
    }
}
