namespace Webserver.IO
{
    public interface IFileReader
    {
        byte[] Read(string path);
    }
}
