using Webserver.Request;

namespace Webserver.Response
{
    public interface IResponseCreator
    {
        byte[] Create(RequestData requestData);

        byte[] Create(ResponseStatusLine responseStatusLine);
    }
}
