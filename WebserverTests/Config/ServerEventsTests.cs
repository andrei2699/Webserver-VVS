using Webserver.Config;
using Xunit;

namespace WebserverTests.Config
{
    public class ServerEventsTests
    {
        private readonly ServerEvents _sut;

        public ServerEventsTests()
        {
            _sut = new ServerEvents();
        }

        [Fact]
        public void ChangeFilePath_ShouldTriggerEvent()
        {
            var called = false;

            _sut.OnFilePathChanged += path =>
            {
                Assert.Equal("somePath", path);
                called = true;
            };

            Assert.False(called);
            _sut.ChangeFilePath("somePath");
            Assert.True(called);
        }

        [Theory]
        [InlineData(ServerState.Maintenance)]
        [InlineData(ServerState.Running)]
        [InlineData(ServerState.Stopped)]
        public void ChangeState_ShouldTriggerEvent(ServerState state)
        {
            var called = false;

            _sut.OnStatusChanged += s =>
            {
                Assert.Equal(s, state);
                called = true;
            };

            Assert.False(called);
            _sut.ChangeState(state);
            Assert.True(called);
        }
    }
}
