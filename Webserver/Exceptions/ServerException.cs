using System;
using System.Net;

namespace Webserver.Exceptions
{
    public class ServerException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ServerException(HttpStatusCode statusCode) : base(statusCode.ToString())
        {
            StatusCode = statusCode;
        }
    }
}
