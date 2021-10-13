namespace Webserver.Response
{
    public interface IResponseCreator
    {
        byte[] Create(ResponseDataHeader responseDataHeader);
    }
}
