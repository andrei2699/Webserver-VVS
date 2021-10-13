using System.Collections.Generic;
using System.Net;
using System.Text;
using Webserver.Response;
using Xunit;

namespace WebserverTests.Response
{
    public class ResponseHeaderParserTests
    {
        private readonly ResponseHeaderParser _sut;

        public ResponseHeaderParserTests()
        {
            _sut = new ResponseHeaderParser();
        }

        [Fact]
        public void Create_ShouldReturnBytes_WhenGivenResponseWithNullHeaders()
        {
            var expectedBytes = Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\r\n\r\n");

            var bytes = _sut.Parse(new ResponseStatusLine("HTTP/1.1", HttpStatusCode.OK), null);

            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public void Create_ShouldReturnBytes_WhenGivenResponseWithNoHeaders()
        {
            var expectedBytes = Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\r\n\r\n");

            var bytes = _sut.Parse(new ResponseStatusLine("HTTP/1.1", HttpStatusCode.OK),
                new Dictionary<string, string>());

            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public void Create_ShouldReturnBytes_WhenGivenResponseWithOneHeader()
        {
            var expectedBytes =
                Encoding.ASCII.GetBytes("HTTP/1.1 500 Internal Server Error\r\nContent-Type: text/html\r\n\r\n");

            var headers = new Dictionary<string, string>
            {
                { "Content-Type", "text/html" }
            };
            var bytes = _sut.Parse(new ResponseStatusLine("HTTP/1.1", HttpStatusCode.InternalServerError), headers);

            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public void Create_ShouldReturnBytes_WhenGivenResponseWithMultipleHeaders()
        {
            var expectedBytes =
                Encoding.ASCII.GetBytes(
                    "HTTP/2.0 400 Bad Request\r\nContent-Type: text/html\r\nContent-Length: 134\r\n\r\n");

            var headers = new Dictionary<string, string>
            {
                { "Content-Type", "text/html" },
                { "Content-Length", "134" },
            };
            var bytes = _sut.Parse(new ResponseStatusLine("HTTP/2.0", HttpStatusCode.BadRequest), headers);

            Assert.Equal(expectedBytes, bytes);
        }
    }
}
