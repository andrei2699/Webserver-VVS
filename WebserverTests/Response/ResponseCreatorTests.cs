using System.Collections.Generic;
using System.Net;
using System.Text;
using Humanizer;
using Moq;
using Webserver.IO;
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

        public ResponseCreatorTests()
        {
            _sut = new ResponseCreator(_responseHeaderParserMock.Object, _filePathProviderMock.Object,
                _fileReaderMock.Object);
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
            var expectedBytes =
                Encoding.ASCII.GetBytes(
                    "HTTP/1.1 400 Bad Request\r\nContent-Type: text/html; charset=utf-8\r\n\r\nOoops, Bad Request!\r\n");

            var responseStatusLine = new ResponseStatusLine("HTTP/1.1", HttpStatusCode.BadRequest);

            _responseHeaderParserMock.Setup(parser => parser.Parse(responseStatusLine, new Dictionary<string, string>
                {
                    { "Content-Type", "text/html; charset=utf-8" }
                }))
                .Returns(Encoding.ASCII.GetBytes(
                    "HTTP/1.1 400 Bad Request\r\nContent-Type: text/html; charset=utf-8\r\n\r\n"));

            _filePathProviderMock.Setup(provider => provider.Provide("400.html")).Returns("path/400.html");
            _fileReaderMock.Setup(reader => reader.Read("path/400.html"))
                .Returns(Encoding.ASCII.GetBytes("Ooops, Bad Request!\r\n"));

            var bytes = _sut.Create(responseStatusLine);

            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public void Create_ShouldCreateByteArray_WhenGivenNotFoundStatusCode()
        {
            var expectedBytes =
                Encoding.ASCII.GetBytes(
                    "HTTP/1.1 404 Not Found\r\nContent-Type: text/html; charset=utf-8\r\n\r\nOoops, file does not exist!\r\n");

            var responseStatusLine = new ResponseStatusLine("HTTP/1.1", HttpStatusCode.NotFound);

            _responseHeaderParserMock.Setup(parser => parser.Parse(responseStatusLine, new Dictionary<string, string>
                {
                    { "Content-Type", "text/html; charset=utf-8" }
                }))
                .Returns(Encoding.ASCII.GetBytes(
                    "HTTP/1.1 404 Not Found\r\nContent-Type: text/html; charset=utf-8\r\n\r\n"));

            _filePathProviderMock.Setup(provider => provider.Provide("404.html")).Returns("path/404.html");
            _fileReaderMock.Setup(reader => reader.Read("path/404.html"))
                .Returns(Encoding.ASCII.GetBytes("Ooops, file does not exist!\r\n"));

            var bytes = _sut.Create(responseStatusLine);

            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public void Create_ShouldCreateByteArray_WhenGivenMethodNotAllowedStatusCode()
        {
            var expectedBytes =
                Encoding.ASCII.GetBytes(
                    "HTTP/1.1 405 Method Not Allowed\r\nContent-Type; text/html: charset=utf-8\r\n\r\nOoops, Method not allowed!\r\n");

            var responseStatusLine = new ResponseStatusLine("HTTP/1.1", HttpStatusCode.MethodNotAllowed);

            _responseHeaderParserMock.Setup(parser => parser.Parse(responseStatusLine, new Dictionary<string, string>
                {
                    { "Content-Type", "text/html; charset=utf-8" }
                }))
                .Returns(Encoding.ASCII.GetBytes(
                    "HTTP/1.1 405 Method Not Allowed\r\nContent-Type; text/html: charset=utf-8\r\n\r\n"));

            _filePathProviderMock.Setup(provider => provider.Provide("405.html")).Returns("path/405.html");
            _fileReaderMock.Setup(reader => reader.Read("path/405.html"))
                .Returns(Encoding.ASCII.GetBytes("Ooops, Method not allowed!\r\n"));

            var bytes = _sut.Create(responseStatusLine);

            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public void Create_ShouldCreateByteArray_WhenGivenInternalServerErrorStatusCode()
        {
            var expectedBytes =
                Encoding.ASCII.GetBytes(
                    "HTTP/1.1 500 InternalServerError\r\nContent-Type: text/html; charset=utf-8\r\n\r\nOoops, something went wrong on the server!\r\n");

            var responseStatusLine = new ResponseStatusLine("HTTP/1.1", HttpStatusCode.InternalServerError);
            _responseHeaderParserMock.Setup(parser => parser.Parse(responseStatusLine, new Dictionary<string, string>
                {
                    { "Content-Type", "text/html; charset=utf-8" }
                }))
                .Returns(Encoding.ASCII.GetBytes(
                    "HTTP/1.1 500 InternalServerError\r\nContent-Type: text/html; charset=utf-8\r\n\r\n"));

            _filePathProviderMock.Setup(provider => provider.Provide("500.html")).Returns("path/500.html");
            _fileReaderMock.Setup(reader => reader.Read("path/500.html"))
                .Returns(Encoding.ASCII.GetBytes("Ooops, something went wrong on the server!\r\n"));

            var bytes = _sut.Create(responseStatusLine);

            Assert.Equal(expectedBytes, bytes);
        }
    }
}
