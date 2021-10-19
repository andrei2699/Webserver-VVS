namespace Webserver.Response
{
    public interface IConfigRequestHandler
    {
        string Handle(string requestDataBody);
    }
}
