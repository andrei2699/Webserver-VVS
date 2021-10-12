namespace Webserver.Request
{
    public interface IRequestParser
    {
        RequestData Parse(byte[] requestBytes);
    }
}
