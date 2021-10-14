namespace Webserver.HTTPHeaders
{
    public interface IContentTypeHeaderProvider
    {
        string Provide(string fileName);
    }
}
