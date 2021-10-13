using System.Collections.Generic;
using System.Net;

namespace Webserver.Response
{
    public record ResponseDataHeader(string Version, HttpStatusCode StatusCode, IDictionary<string, string> Headers);
}
