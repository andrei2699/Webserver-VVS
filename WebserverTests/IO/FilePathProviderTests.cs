using System.IO;
using Moq;
using Webserver.IO;
using Xunit;

namespace WebserverTests.IO
{
    public class FilePathProviderTests
    {
        private readonly FilePathProvider _sut;

        private readonly Mock<IFilePathValidator> _filePathValidatorMock = new();

        public FilePathProviderTests()
        {
            _sut = new FilePathProvider(_filePathValidatorMock.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        public void Provide_ShouldReturnEmptyString_WhenGivenEmptyPath(string path)
        {
            var providedPath = _sut.Provide(path);

            Assert.Empty(providedPath);
        }

        [Theory]
        [InlineData("400.html")]
        [InlineData("404.html")]
        [InlineData("405.html")]
        [InlineData("500.html")]
        public void Provide_ShouldReturnDefaultPage_WhenGivenDefaultPageName_AndRootFolderIsNotSet(
            string defaultPageName)
        {
            var providedPath = _sut.Provide(defaultPageName);

            Assert.EndsWith(Path.Combine("DefaultPages", defaultPageName), providedPath);
        }

        [Theory]
        [InlineData("400.html")]
        [InlineData("404.html")]
        [InlineData("405.html")]
        [InlineData("500.html")]
        public void Provide_ShouldReturnDefaultPage_WhenGivenDefaultPageName_AndRootFolderHasNoDefaultPage(
            string defaultPageName)
        {
            _sut.SetRootPath("rootPath");

            var providedPath = _sut.Provide(defaultPageName);

            Assert.EndsWith(Path.Combine("DefaultPages", defaultPageName), providedPath);
        }

        [Theory]
        [InlineData("400.html")]
        [InlineData("404.html")]
        [InlineData("405.html")]
        [InlineData("500.html")]
        public void Provide_ShouldReturnDefaultPage_WhenGivenDefaultPageName_AndRootFolderHasDefaultPage(
            string defaultPageName)
        {
            var expectedPath = Path.Combine("rootPath", defaultPageName);

            _sut.SetRootPath("rootPath");
            _filePathValidatorMock.Setup(validator => validator.Validate(expectedPath)).Returns(true);

            var providedPath = _sut.Provide(defaultPageName);

            Assert.Equal(expectedPath, providedPath);
        }

        [Fact]
        public void Provide_ShouldReturnTheSamePath_WhenRootFolderIsNotSet()
        {
            var providedPath = _sut.Provide("SomePath/file.html");

            Assert.Equal("SomePath/file.html", providedPath);
        }

        [Fact]
        public void Provide_ShouldReturnThePathWithRootFolder_WhenRootFolderIsSet()
        {
            _sut.SetRootPath(Path.Combine("path", "to", "root"));

            var providedPath = _sut.Provide("file.html");

            Assert.Equal(Path.Combine("path", "to", "root", "file.html"), providedPath);
        }

        [Fact]
        public void Provide_ShouldReturnThePathWithRootFolder_WhenRootFolderIsSet_AndPathBeginsWithSlash()
        {
            _sut.SetRootPath("path_to_root");

            var providedPath = _sut.Provide("/file.html");

            Assert.Equal(Path.Combine("path_to_root", "file.html"), providedPath);
        }

        [Fact]
        public void Provide_ShouldReturnThePathWithRootFolder_WhenRootFolderIsSet_AndPathBeginsWithBackSlash()
        {
            _sut.SetRootPath("path_to_root");

            var providedPath = _sut.Provide("\\file.html");

            Assert.Equal(Path.Combine("path_to_root", "file.html"), providedPath);
        }

        [Fact]
        public void Provide_ShouldReturnThePathWithRootFolder_WhenRootFolderEndsWithSlash()
        {
            _sut.SetRootPath("path_to_root/");

            var providedPath = _sut.Provide("file.html");

            Assert.Equal(Path.Combine("path_to_root", "file.html"), providedPath);
        }

        [Fact]
        public void Provide_ShouldReturnThePathWithRootFolder_WhenRootFolderEndsWithBackSlash()
        {
            _sut.SetRootPath("path_to_root\\");

            var providedPath = _sut.Provide("file.html");

            Assert.Equal(Path.Combine("path_to_root", "file.html"), providedPath);
        }

        [Fact]
        public void Provide_ShouldReturnThePathOfIndexHtml_WhenRootFolderIsGiven_AndIsGivenRootAsPath()
        {
            _sut.SetRootPath("path_to_root");

            var providedPath = _sut.Provide("/");

            Assert.Equal(Path.Combine("path_to_root", "index.html"), providedPath);
        }

        [Fact]
        public void Provide_ShouldReturnThePathOfIndexHtml_WhenRootFolderIsNotGiven_AndIsGivenRootAsPath()
        {
            var providedPath = _sut.Provide("/");

            Assert.Equal("index.html", providedPath);
        }
    }
}
