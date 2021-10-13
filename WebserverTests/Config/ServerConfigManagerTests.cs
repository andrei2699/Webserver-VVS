using System.IO;
using System.Text;
using Moq;
using Webserver.Config;
using Webserver.IO;
using Xunit;

namespace WebserverTests.Config
{
    public class ServerConfigManagerTests
    {
        private readonly ServerConfigManager _sut;

        private readonly Mock<IFileReader> _fileReaderMock = new();
        private readonly Mock<IFileWriter> _fileWriterMock = new();

        public ServerConfigManagerTests()
        {
            _sut = new ServerConfigManager(_fileReaderMock.Object, _fileWriterMock.Object);
        }

        [Fact]
        public void ReadConfig_ShouldReturnConfig_WhenConfigFileExists()
        {
            _fileReaderMock.Setup(reader => reader.Read("config.json"))
                .Returns(Encoding.ASCII.GetBytes(@"{""Port"":5125,""FilePath"":""a_file/path""}"));

            var (port, filePath) = _sut.ReadConfig();

            Assert.Equal(5125, port);
            Assert.Equal("a_file/path", filePath);
        }

        [Fact]
        public void ReadConfig_ShouldCreateNewConfig_WhenConfigFileDoesNotExist()
        {
            _fileReaderMock.Setup(reader => reader.Read("config.json")).Throws<FileNotFoundException>();

            var (port, filePath) = _sut.ReadConfig();

            _fileWriterMock.Verify(writer => writer.Write("config.json", It.IsAny<string>()), Times.Once());

            Assert.Equal(8080, port);
            Assert.False(string.IsNullOrEmpty(filePath));
        }

        [Fact]
        public void ReadConfig_ShouldCreateNewConfig_WhenConfigFileIsNotJson()
        {
            _fileReaderMock.Setup(reader => reader.Read("config.json")).Returns(Encoding.ASCII.GetBytes("text"));

            var (port, filePath) = _sut.ReadConfig();

            _fileWriterMock.Verify(writer => writer.Write("config.json", It.IsAny<string>()), Times.Once());

            Assert.Equal(8080, port);
            Assert.False(string.IsNullOrEmpty(filePath));
        }

        [Fact]
        public void WriteConfig_ShouldNotWrite_WhenGivenNullConfig()
        {
            _sut.WriteConfig(null);

            _fileWriterMock.Verify(writer => writer.Write("config.json", null), Times.Never);
        }

        [Fact]
        public void WriteConfig_ShouldWriteConfig_WhenGivenConfig()
        {
            _sut.WriteConfig(new ServerConfig(1234, "somePath"));

            _fileWriterMock.Verify(writer => writer.Write("config.json", @"{""Port"":1234,""FilePath"":""somePath""}"),
                Times.Once());
        }
    }
}
