using System.Collections.Generic;

namespace Webserver.Request
{
    public record RequestData(string Method, string Target, string Version, IDictionary<string, string> Headers,
        byte[] Body = null);
}
