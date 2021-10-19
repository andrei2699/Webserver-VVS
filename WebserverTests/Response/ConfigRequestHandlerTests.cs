using System;
using System.Text.Json;
using Moq;
using Webserver.Config;
using Webserver.Response;
using Xunit;

namespace WebserverTests.Response
{
    public class ConfigRequestHandlerTests
    {
        private readonly ConfigRequestHandler _sut;

        private readonly Mock<IServerEvents> _serverManagerMock = new();

        public ConfigRequestHandlerTests()
        {
            _sut = new ConfigRequestHandler(_serverManagerMock.Object);
        }

        [Fact]
        public void Handle_ShouldThrowJsonException_WhenGivenInvalidJsonFile()
        {
            Assert.Throws<JsonException>(() => _sut.Handle("not json"));
        }

        [Theory]
        [InlineData(@"{""action2"":""change_path"",""value"":""other_path""}")]
        [InlineData(@"{""action"":""change_path"",""value2"":""other_path""}")]
        [InlineData(@"{""action2"":""change_path"",""value2"":""other_path""}")]
        public void Handle_ShouldThrowArgumentException_WhenGivenJsonFileWithWrongData(string value)
        {
            Assert.Throws<ArgumentException>(() => _sut.Handle(value));
        }

        [Fact]
        public void Handle_ShouldReturnPathChanged_WhenGivenRequestForPathChange()
        {
            const string expectedString = @"{""success"":true,""message"":""path changed""}";

            var responseString = _sut.Handle(@"{""action"":""change_path"",""value"":""other_path""}");


            _serverManagerMock.Verify(manager => manager.ChangeState(It.IsAny<ServerState>()), Times.Never);
            _serverManagerMock.Verify(manager => manager.ChangeFilePath("other_path"), Times.Once);

            Assert.Equal(expectedString, responseString);
        }

        [Theory]
        [InlineData(@"{""action"":""change_state"",""value"":""52""}")]
        [InlineData(@"{""action"":""change_state"",""value"":""some_string""}")]
        public void Handle_ShouldReturnInvalidStatus_WhenGivenRequestForStatusChange(string value)
        {
            const string expectedString = @"{""success"":false,""message"":""invalid status""}";

            var responseString = _sut.Handle(value);

            _serverManagerMock.Verify(manager => manager.ChangeState(It.IsAny<ServerState>()), Times.Never);
            _serverManagerMock.Verify(manager => manager.ChangeFilePath(It.IsAny<string>()), Times.Never);

            Assert.Equal(expectedString, responseString);
        }

        [Fact]
        public void Handle_ShouldReturnInvalidCommand_WhenGivenRequestFromInvalidCommand()
        {
            const string expectedString = @"{""success"":false,""message"":""invalid command""}";

            var responseString = _sut.Handle(@"{""action"":""other_command"",""value"":""2""}");


            _serverManagerMock.Verify(manager => manager.ChangeState(It.IsAny<ServerState>()), Times.Never);
            _serverManagerMock.Verify(manager => manager.ChangeFilePath(It.IsAny<string>()), Times.Never);

            Assert.Equal(expectedString, responseString);
        }

        [Fact]
        public void Handle_ShouldReturnStatusChangedToMaintenance_WhenGivenRequestForStatusChange()
        {
            const string expectedString = @"{""success"":true,""message"":""status changed to maintenance""}";

            var responseString = _sut.Handle(@"{""action"":""change_state"",""value"":""2""}");

            _serverManagerMock.Verify(manager => manager.ChangeState(ServerState.Maintenance), Times.Once);
            _serverManagerMock.Verify(manager => manager.ChangeFilePath(It.IsAny<string>()), Times.Never);

            Assert.Equal(expectedString, responseString);
        }

        [Fact]
        public void Handle_ShouldReturnStatusChangedToRunning_WhenGivenRequestForStatusChange()
        {
            const string expectedString = @"{""success"":true,""message"":""status changed to running""}";

            var responseString = _sut.Handle(@"{""action"":""change_state"",""value"":""1""}");

            _serverManagerMock.Verify(manager => manager.ChangeState(ServerState.Running), Times.Once);
            _serverManagerMock.Verify(manager => manager.ChangeFilePath(It.IsAny<string>()), Times.Never);

            Assert.Equal(expectedString, responseString);
        }

        [Fact]
        public void Handle_ShouldReturnStoppingServer_WhenGivenRequestForStatusChange()
        {
            const string expectedString = @"{""success"":true,""message"":""stopping server""}";

            var responseString = _sut.Handle(@"{""action"":""change_state"",""value"":""0""}");

            _serverManagerMock.Verify(manager => manager.ChangeState(ServerState.Stopped), Times.Once);
            _serverManagerMock.Verify(manager => manager.ChangeFilePath(It.IsAny<string>()), Times.Never);

            Assert.Equal(expectedString, responseString);
        }
    }
}
