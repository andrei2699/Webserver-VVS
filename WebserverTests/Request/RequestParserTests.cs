using System.Net;
using System.Text;
using Webserver.Exceptions;
using Webserver.Request;
using Xunit;

namespace WebserverTests.Request
{
    public class RequestParserTests
    {
        private readonly RequestParser _sut;

        public RequestParserTests()
        {
            _sut = new RequestParser();
        }

        [Fact]
        public void Parse_ShouldThrowBadRequestException_WhenGivenNullRequest()
        {
            var serverException = Assert.Throws<ServerException>(() => _sut.Parse(null));
            Assert.Equal(HttpStatusCode.BadRequest, serverException.StatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData("\r\n")]
        [InlineData("\t")]
        public void Parse_ShouldThrowBadRequestException_WhenGivenEmptyRequest(string requestContent)
        {
            var serverException =
                Assert.Throws<ServerException>(() => _sut.Parse(Encoding.ASCII.GetBytes(requestContent)));
            Assert.Equal(HttpStatusCode.BadRequest, serverException.StatusCode);
        }


        [Theory]
        [InlineData("POST")]
        [InlineData("GET /")]
        [InlineData("GET /index.html HTTP/1.1")]
        [InlineData("GET /index.html HTTP/1.1\r\n")]
        [InlineData("  \r\n")]
        [InlineData("GET /index.html HTTP/1.1\r\nHost: localhost:8080")]
        [InlineData("UPDATE / HTTP/1.1\r\nHost: localhost:8080\r\n")]
        [InlineData("UPDATE / HTTP/1.1\r\nHost: localhost:8080\r\n\r")]
        [InlineData("UPDATE / HTTP/1.1\r\nHost localhost\r\n\r\n")]
        [InlineData("UPDATE / HTTP/1.1\rHost: localhost:8080\r\r")]
        [InlineData("UPDATE / HTTP/1.1\nHost: localhost:8080\n\n")]
        public void Parse_ShouldThrowBadRequestException_WhenGivenWrongRequest(string requestContent)
        {
            var serverException =
                Assert.Throws<ServerException>(() => _sut.Parse(Encoding.ASCII.GetBytes(requestContent)));
            Assert.Equal(HttpStatusCode.BadRequest, serverException.StatusCode);
        }

        [Fact]
        public void Parse_ShouldReturnRequestData_WhenProvidedWithRequestWithNoHeadersAndNoBody()
        {
            const string requestContent = "GET / HTTP/1.1\r\n\r\n";

            var requestData = _sut.Parse(Encoding.ASCII.GetBytes(requestContent));

            Assert.Equal("GET", requestData.Method);
            Assert.Equal("/", requestData.Target);
            Assert.Equal("HTTP/1.1", requestData.Version);
            Assert.Empty(requestData.Headers);
            Assert.Null(requestData.Body);
        }

        [Fact]
        public void Parse_ShouldReturnRequestData_WhenProvidedWithRequestWithOneHeaderAndNoBody()
        {
            const string requestContent = "GET / HTTP/1.1\r\nHost: localhost:8901\r\n\r\n";

            var requestData = _sut.Parse(Encoding.ASCII.GetBytes(requestContent));

            Assert.Equal("GET", requestData.Method);
            Assert.Equal("/", requestData.Target);
            Assert.Equal("HTTP/1.1", requestData.Version);
            Assert.Equal(1, requestData.Headers.Count);
            Assert.Equal("localhost:8901", requestData.Headers["Host"]);
            Assert.Null(requestData.Body);
        }

        [Fact]
        public void Parse_ShouldReturnRequestData_WhenProvidedWithRequestWithMultipleHeadersAndNoBody()
        {
            const string requestContent =
                "POST /index.html HTTP/2.0\r\nHost: localhost:8901\r\nAccept: text/html\r\n\r\n";

            var requestData = _sut.Parse(Encoding.ASCII.GetBytes(requestContent));

            Assert.Equal("POST", requestData.Method);
            Assert.Equal("/index.html", requestData.Target);
            Assert.Equal("HTTP/2.0", requestData.Version);
            Assert.Equal(2, requestData.Headers.Count);
            Assert.Equal("localhost:8901", requestData.Headers["Host"]);
            Assert.Equal("text/html", requestData.Headers["Accept"]);
            Assert.Null(requestData.Body);
        }

        [Fact]
        public void Parse_ShouldReturnRequestData_WhenProvidedWithRequestWithMultipleHeadersThatRepeatAndNoBody()
        {
            const string requestContent =
                "GET / HTTP/1.1\r\nHost: localhost:8901\r\nContent-Length: 345\r\nHost: localhost:1234\r\n\r\n";

            var requestData = _sut.Parse(Encoding.ASCII.GetBytes(requestContent));

            Assert.Equal("GET", requestData.Method);
            Assert.Equal("/", requestData.Target);
            Assert.Equal("HTTP/1.1", requestData.Version);
            Assert.Equal(2, requestData.Headers.Count);
            Assert.Equal("localhost:1234", requestData.Headers["Host"]);
            Assert.Equal("345", requestData.Headers["Content-Length"]);
            Assert.Null(requestData.Body);
        }

        [Fact]
        public void Parse_ShouldReturnRequestData_WhenProvidedWithRequestWithNoHeadersAndBody()
        {
            const string requestContent = "POST /index.html HTTP/2.0\r\n\r\n{\"Some\":\"Body\"}";

            var requestData = _sut.Parse(Encoding.ASCII.GetBytes(requestContent));

            Assert.Equal("POST", requestData.Method);
            Assert.Equal("/index.html", requestData.Target);
            Assert.Equal("HTTP/2.0", requestData.Version);
            Assert.Empty(requestData.Headers);
            Assert.Equal(Encoding.ASCII.GetBytes("{\"Some\":\"Body\"}"), requestData.Body);
        }

        [Fact]
        public void Parse_ShouldReturnRequestData_WhenProvidedWithRequestWithMultipleHeadersAndBody()
        {
            const string requestContent =
                "POST /index.html HTTP/2.0\r\nHost: localhost:8901\r\nAccept: text/html\r\n\r\nSome Body";

            var requestData = _sut.Parse(Encoding.ASCII.GetBytes(requestContent));

            Assert.Equal("POST", requestData.Method);
            Assert.Equal("/index.html", requestData.Target);
            Assert.Equal("HTTP/2.0", requestData.Version);
            Assert.Equal(2, requestData.Headers.Count);
            Assert.Equal("localhost:8901", requestData.Headers["Host"]);
            Assert.Equal("text/html", requestData.Headers["Accept"]);
            Assert.Equal(Encoding.ASCII.GetBytes("Some Body"), requestData.Body);
        }
    }
}
