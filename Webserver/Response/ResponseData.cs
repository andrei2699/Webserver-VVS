using System.Collections.Generic;
using System.Net;

namespace Webserver.Response
{
    public record ResponseData(string Version, HttpStatusCode StatusCode, IDictionary<string, string> Headers,
        byte[] Body = null);
}
