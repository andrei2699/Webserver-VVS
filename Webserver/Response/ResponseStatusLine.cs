using System.Net;

namespace Webserver.Response
{
    public record ResponseStatusLine(string Version, HttpStatusCode StatusCode);
}
