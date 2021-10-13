namespace Webserver.Response
{
    public interface IResponseCreator
    {
        byte[] Create(ResponseData responseData);
    }
}
