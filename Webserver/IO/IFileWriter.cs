namespace Webserver.IO
{
    public interface IFileWriter
    {
        void Write(string path, string content);
    }
}
