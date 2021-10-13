using System.Collections.Generic;

namespace Webserver.Response
{
    public interface IResponseHeaderParser
    {
        byte[] Parse(ResponseStatusLine responseStatusLine, IDictionary<string, string> headers = null);
    }
}
