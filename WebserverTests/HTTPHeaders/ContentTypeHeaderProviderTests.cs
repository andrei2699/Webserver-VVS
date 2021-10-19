using Webserver.HTTPHeaders;
using Xunit;

namespace WebserverTests.HTTPHeaders
{
    public class ContentTypeHeaderProviderTests
    {
        private readonly ContentTypeHeaderProvider _sut;

        public ContentTypeHeaderProviderTests()
        {
            _sut = new ContentTypeHeaderProvider();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData("\r\n")]
        [InlineData("\t")]
        public void Provide_ShouldReturnTextPlain_WhenGivenEmptyString(string fileName)
        {
            var header = _sut.Provide(fileName);

            Assert.Equal("text/plain; charset=utf-8", header);
        }

        [Theory]
        [InlineData("file.tar")]
        [InlineData("folder/arch.zip")]
        public void Provide_ShouldReturnTextPlain_WhenGivenFileWithUnknownExtension(string fileName)
        {
            var header = _sut.Provide(fileName);

            Assert.Equal("text/plain; charset=utf-8", header);
        }

        [Fact]
        public void Provide_ShouldReturnTextPlain_WhenGivenFileWithoutExtension()
        {
            var header = _sut.Provide("Path/SomeFileName");

            Assert.Equal("text/plain; charset=utf-8", header);
        }

        [Fact]
        public void Provide_ShouldReturnTextCss_WhenGivenCssFile()
        {
            var header = _sut.Provide("Path/style.css");

            Assert.Equal("text/css; charset=utf-8", header);
        }

        [Fact]
        public void Provide_ShouldReturnTextCsv_WhenGivenCsvFile()
        {
            var header = _sut.Provide("Path/data.csv");

            Assert.Equal("text/csv; charset=utf-8", header);
        }

        [Fact]
        public void Provide_ShouldReturnTextXml_WhenGivenXmlFile()
        {
            var header = _sut.Provide("Path/config.xml");

            Assert.Equal("text/xml; charset=utf-8", header);
        }

        [Theory]
        [InlineData("html")]
        [InlineData("htm")]
        public void Provide_ShouldReturnTextHtml_WhenGivenHtmlFile(string extension)
        {
            var header = _sut.Provide($"file.{extension}");

            Assert.Equal("text/html; charset=utf-8", header);
        }

        [Fact]
        public void Provide_ShouldReturnTextHtml_WhenGivenRoot()
        {
            var header = _sut.Provide("/");

            Assert.Equal("text/html; charset=utf-8", header);
        }

        
        [Fact]
        public void Provide_ShouldReturnImagePng_WhenGivenPng()
        {
            var header = _sut.Provide("images/img.png");

            Assert.Equal("image/png", header);
        }

        [Fact]
        public void Provide_ShouldReturnImageGif_WhenGivenPng()
        {
            var header = _sut.Provide("images/img.gif");

            Assert.Equal("image/gif", header);
        }

        [Fact]
        public void Provide_ShouldReturnImageTiff_WhenGivenTiff()
        {
            var header = _sut.Provide("images/img.tiff");

            Assert.Equal("image/tiff", header);
        }

        [Theory]
        [InlineData("jpg")]
        [InlineData("jpeg")]
        public void Provide_ShouldReturnImageJpg_WhenGivenJpgImage(string extension)
        {
            var header = _sut.Provide($"images/img.{extension}");

            Assert.Equal("image/jpeg", header);
        }
    }
}
