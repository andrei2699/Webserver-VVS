using System.Collections.Generic;
using System.Net;
using System.Text;
using Webserver.Response;
using Xunit;

namespace WebserverTests.Response
{
    public class ResponseCreatorTests
    {
        private readonly ResponseCreator _sut;

        public ResponseCreatorTests()
        {
            _sut = new ResponseCreator();
        }

        [Fact]
        public void Create_ShouldReturnBytes_WhenGivenResponseWithNullHeadersAndNoBody()
        {
            var expectedBytes = Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\r\n\r\n");

            var bytes = _sut.Create(new ResponseData("HTTP/1.1", HttpStatusCode.OK, null));

            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public void Create_ShouldReturnBytes_WhenGivenResponseWithNoHeadersAndNoBody()
        {
            var expectedBytes = Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\r\n\r\n");

            var bytes = _sut.Create(new ResponseData("HTTP/1.1", HttpStatusCode.OK, new Dictionary<string, string>()));

            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public void Create_ShouldReturnBytes_WhenGivenResponseWithOneHeaderAndNoBody()
        {
            var expectedBytes =
                Encoding.ASCII.GetBytes("HTTP/1.1 500 Internal Server Error\r\nContent-Type: text/html\r\n\r\n");

            var headers = new Dictionary<string, string>
            {
                { "Content-Type", "text/html" }
            };
            var bytes = _sut.Create(new ResponseData("HTTP/1.1", HttpStatusCode.InternalServerError, headers));

            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public void Create_ShouldReturnBytes_WhenGivenResponseWithMultipleHeadersAndNoBody()
        {
            var expectedBytes =
                Encoding.ASCII.GetBytes(
                    "HTTP/2.0 400 Bad Request\r\nContent-Type: text/html\r\nContent-Length: 134\r\n\r\n");

            var headers = new Dictionary<string, string>
            {
                { "Content-Type", "text/html" },
                { "Content-Length", "134" },
            };
            var bytes = _sut.Create(new ResponseData("HTTP/2.0", HttpStatusCode.BadRequest, headers));

            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public void Create_ShouldReturnBytes_WhenGivenResponseWithMultipleHeadersAndBody()
        {
            var expectedBytes =
                Encoding.ASCII.GetBytes(
                    "HTTP/2.0 400 Bad Request\r\nContent-Type: text/html\r\nContent-Length: 134\r\n\r\n<html><body></body></html>");

            var headers = new Dictionary<string, string>
            {
                { "Content-Type", "text/html" },
                { "Content-Length", "134" },
            };
            var bodyBytes = Encoding.ASCII.GetBytes("<html><body></body></html>");
            var bytes = _sut.Create(new ResponseData("HTTP/2.0", HttpStatusCode.BadRequest, headers, bodyBytes));

            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public void Create_ShouldReturnBytes_WhenGivenResponseWithNoHeadersAndBody()
        {
            var expectedBytes = Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\r\n\r\n<html><body></body></html>");

            var bodyBytes = Encoding.ASCII.GetBytes("<html><body></body></html>");
            var bytes = _sut.Create(new ResponseData("HTTP/1.1", HttpStatusCode.OK, new Dictionary<string, string>(),
                bodyBytes));

            Assert.Equal(expectedBytes, bytes);
        }
    }
}
