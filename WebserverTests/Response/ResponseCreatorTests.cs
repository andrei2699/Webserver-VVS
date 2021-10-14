using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Humanizer;
using Moq;
using Webserver.HTTPHeaders;
using Webserver.IO;
using Webserver.Request;
using Webserver.Response;
using Xunit;

namespace WebserverTests.Response
{
    public class ResponseCreatorTests
    {
        private readonly ResponseCreator _sut;

        private readonly Mock<IResponseHeaderParser> _responseHeaderParserMock = new();
        private readonly Mock<IFilePathProvider> _filePathProviderMock = new();
        private readonly Mock<IFileReader> _fileReaderMock = new();
        private readonly Mock<IContentTypeHeaderProvider> _contentTypeHeaderProviderMock = new();

        public ResponseCreatorTests()
        {
            _sut = new ResponseCreator(_responseHeaderParserMock.Object, _filePathProviderMock.Object,
                _fileReaderMock.Object, _contentTypeHeaderProviderMock.Object);
        }

        [Theory]
        [InlineData(HttpStatusCode.Gone)]
        [InlineData(HttpStatusCode.Conflict)]
        [InlineData(HttpStatusCode.GatewayTimeout)]
        public void Create_ShouldCreateByteArray_WhenGivenStatusCodeWithNoDefaultPage(HttpStatusCode statusCode)
        {
            var expectedBytes =
                Encoding.ASCII.GetBytes(
                    $"HTTP/1.1 {(int)statusCode} {statusCode.Humanize(LetterCasing.Title)}\r\n\r\n");
            var responseStatusLine = new ResponseStatusLine("HTTP/1.1", statusCode);

            _responseHeaderParserMock.Setup(parser => parser.Parse(responseStatusLine, null))
                .Returns(expectedBytes);

            var bytes = _sut.Create(responseStatusLine);

            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public void Create_ShouldCreateByteArray_WhenGivenBadRequestStatusCode()
        {
            var responseStatusLine = new ResponseStatusLine("HTTP/1.1", HttpStatusCode.BadRequest);
            var expectedBytes = GetBytesForBadRequest(responseStatusLine);

            var bytes = _sut.Create(responseStatusLine);

            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public void Create_ShouldCreateByteArray_WhenGivenNotFoundStatusCode()
        {
            var responseStatusLine = new ResponseStatusLine("HTTP/1.1", HttpStatusCode.NotFound);
            var expectedBytes = GetBytesForNotFound(responseStatusLine);

            var bytes = _sut.Create(responseStatusLine);

            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public void Create_ShouldCreateByteArray_WhenGivenMethodNotAllowedStatusCode()
        {
            var responseStatusLine = new ResponseStatusLine("HTTP/1.1", HttpStatusCode.MethodNotAllowed);
            var expectedBytes = GetBytesForNotAllowedMethod(responseStatusLine);

            var bytes = _sut.Create(responseStatusLine);

            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public void Create_ShouldCreateByteArray_WhenGivenInternalServerErrorStatusCode()
        {
            var responseStatusLine = new ResponseStatusLine("HTTP/1.1", HttpStatusCode.InternalServerError);
            var expectedBytes = GetBytesForInternalServerError(responseStatusLine);

            var bytes = _sut.Create(responseStatusLine);

            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public void Create_ShouldCreateByteArray_WhenGivenNullRequestData()
        {
            var expectedBytes = GetBytesForBadRequest(new ResponseStatusLine("HTTP/1.1", HttpStatusCode.BadRequest));

            var bytes = _sut.Create((RequestData)null);

            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public void Create_ShouldCreateByteArray_WhenGivenRequestWithTargetThatIsNotFound()
        {
            var expectedBytes = GetBytesForNotFound(new ResponseStatusLine("HTTP/1.1", HttpStatusCode.NotFound));

            _filePathProviderMock.Setup(provider => provider.Provide("someFile.txt")).Returns("path/someFile.txt");
            _fileReaderMock.Setup(reader => reader.Read("path/someFile.txt"))
                .Throws<FileNotFoundException>();

            var bytes = _sut.Create(new RequestData("GET", "someFile.txt", "HTTP/1.1", null));

            Assert.Equal(expectedBytes, bytes);
        }

        [Theory]
        [InlineData("POST")]
        [InlineData("DELETE")]
        [InlineData("PUT")]
        public void Create_ShouldCreateByteArray_WhenGivenRequestWithNotAllowedMethod(string method)
        {
            var expectedBytes =
                GetBytesForNotAllowedMethod(new ResponseStatusLine("HTTP/1.1", HttpStatusCode.MethodNotAllowed));

            _filePathProviderMock.Setup(provider => provider.Provide("someFile.txt")).Returns("path/someFile.txt");
            _fileReaderMock.Setup(reader => reader.Read("path/someFile.txt"))
                .Throws<FileNotFoundException>();

            var bytes = _sut.Create(new RequestData(method, "someFile.txt", "HTTP/1.1", null));

            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public void Create_ShouldCreateByteArray_WhenGivenRequestThatGeneratesOtherKindOfException()
        {
            var expectedBytes =
                GetBytesForInternalServerError(new ResponseStatusLine("HTTP/1.1", HttpStatusCode.InternalServerError));

            _filePathProviderMock.Setup(provider => provider.Provide("/images/img.png")).Returns("path/images/img.png");
            _fileReaderMock.Setup(reader => reader.Read("path/images/img.png")).Throws<FileLoadException>();


            var bytes = _sut.Create(new RequestData("GET", "/images/img.png", "HTTP/1.1", null));

            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public void Create_ShouldCreateByteArray_WhenGivenValidRequest()
        {
            var expectedBytes =
                Encoding.ASCII.GetBytes(
                    "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=utf-8\r\n\r\n<html><body><p>Hello</p></body></html>");

            _filePathProviderMock.Setup(provider => provider.Provide("/pages/page1.html"))
                .Returns("path/pages/page1.html");
            _contentTypeHeaderProviderMock.Setup(provider => provider.Provide("/pages/page1.html"))
                .Returns("text/html; charset=utf-8");
            _fileReaderMock.Setup(reader => reader.Read("path/pages/page1.html"))
                .Returns(Encoding.ASCII.GetBytes("<html><body><p>Hello</p></body></html>"));

            _responseHeaderParserMock.Setup(parser => parser.Parse(
                    new ResponseStatusLine("HTTP/1.1", HttpStatusCode.OK), new Dictionary<string, string>
                    {
                        { "Content-Type", "text/html; charset=utf-8" }
                    }))
                .Returns(Encoding.ASCII.GetBytes(
                    "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=utf-8\r\n\r\n"));

            var bytes = _sut.Create(new RequestData("GET", "/pages/page1.html", "HTTP/1.1",
                new Dictionary<string, string>
                {
                    { "Host", "localhost:8080" }
                }));

            Assert.Equal(expectedBytes, bytes);
        }


        private byte[] GetBytesForBadRequest(ResponseStatusLine responseStatusLine)
        {
            var expectedBytes =
                Encoding.ASCII.GetBytes(
                    "HTTP/1.1 400 Bad Request\r\nContent-Type: text/html; charset=utf-8\r\n\r\nOoops, Bad Request!\r\n");

            _responseHeaderParserMock.Setup(parser => parser.Parse(responseStatusLine, new Dictionary<string, string>
                {
                    { "Content-Type", "text/html; charset=utf-8" }
                }))
                .Returns(Encoding.ASCII.GetBytes(
                    "HTTP/1.1 400 Bad Request\r\nContent-Type: text/html; charset=utf-8\r\n\r\n"));

            _filePathProviderMock.Setup(provider => provider.Provide("400.html")).Returns("path/400.html");
            _fileReaderMock.Setup(reader => reader.Read("path/400.html"))
                .Returns(Encoding.ASCII.GetBytes("Ooops, Bad Request!\r\n"));
            return expectedBytes;
        }

        private byte[] GetBytesForNotFound(ResponseStatusLine responseStatusLine)
        {
            _responseHeaderParserMock.Setup(parser => parser.Parse(responseStatusLine, new Dictionary<string, string>
                {
                    { "Content-Type", "text/html; charset=utf-8" }
                }))
                .Returns(Encoding.ASCII.GetBytes(
                    "HTTP/1.1 404 Not Found\r\nContent-Type: text/html; charset=utf-8\r\n\r\n"));

            _filePathProviderMock.Setup(provider => provider.Provide("404.html")).Returns("path/404.html");
            _fileReaderMock.Setup(reader => reader.Read("path/404.html"))
                .Returns(Encoding.ASCII.GetBytes("Ooops, file does not exist!\r\n"));

            var expectedBytes =
                Encoding.ASCII.GetBytes(
                    "HTTP/1.1 404 Not Found\r\nContent-Type: text/html; charset=utf-8\r\n\r\nOoops, file does not exist!\r\n");
            return expectedBytes;
        }

        private byte[] GetBytesForInternalServerError(ResponseStatusLine responseStatusLine)
        {
            var expectedBytes =
                Encoding.ASCII.GetBytes(
                    "HTTP/1.1 500 InternalServerError\r\nContent-Type: text/html; charset=utf-8\r\n\r\nOoops, something went wrong on the server!\r\n");

            _responseHeaderParserMock.Setup(parser => parser.Parse(responseStatusLine, new Dictionary<string, string>
                {
                    { "Content-Type", "text/html; charset=utf-8" }
                }))
                .Returns(Encoding.ASCII.GetBytes(
                    "HTTP/1.1 500 InternalServerError\r\nContent-Type: text/html; charset=utf-8\r\n\r\n"));

            _filePathProviderMock.Setup(provider => provider.Provide("500.html")).Returns("path/500.html");
            _fileReaderMock.Setup(reader => reader.Read("path/500.html"))
                .Returns(Encoding.ASCII.GetBytes("Ooops, something went wrong on the server!\r\n"));

            return expectedBytes;
        }

        private byte[] GetBytesForNotAllowedMethod(ResponseStatusLine responseStatusLine)
        {
            var expectedBytes =
                Encoding.ASCII.GetBytes(
                    "HTTP/1.1 405 Method Not Allowed\r\nContent-Type; text/html: charset=utf-8\r\n\r\nOoops, Method not allowed!\r\n");

            _responseHeaderParserMock.Setup(parser => parser.Parse(responseStatusLine, new Dictionary<string, string>
                {
                    { "Content-Type", "text/html; charset=utf-8" }
                }))
                .Returns(Encoding.ASCII.GetBytes(
                    "HTTP/1.1 405 Method Not Allowed\r\nContent-Type; text/html: charset=utf-8\r\n\r\n"));

            _filePathProviderMock.Setup(provider => provider.Provide("405.html")).Returns("path/405.html");
            _fileReaderMock.Setup(reader => reader.Read("path/405.html"))
                .Returns(Encoding.ASCII.GetBytes("Ooops, Method not allowed!\r\n"));

            return expectedBytes;
        }
    }
}
