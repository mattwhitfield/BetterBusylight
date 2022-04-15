namespace BetterBusylight.Tests
{
    using BetterBusylight;
    using System;
    using Xunit;
    using FluentAssertions;
    using NSubstitute;
    using System.Drawing;

    public class BusylightHandlerTests
    {
        private BusylightHandler _testClass;
        private IDeviceManagement _management;

        public BusylightHandlerTests()
        {
            _management = Substitute.For<IDeviceManagement>();
            _testClass = new BusylightHandler(_management);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new BusylightHandler(_management);
            
            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CannotConstructWithNullManagement()
        {
            FluentActions.Invoking(() => new BusylightHandler(default(IDeviceManagement))).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CanCallLight()
        {
            // Arrange
            var sequence = new Sequence(new[] { new Static(Color.FromArgb(255, 0, 0)) });
            var currentElapsed = 1667902130.73;

            byte[] bytesReceived = null;
            _management.When(x => x.Write(Arg.Any<byte[]>())).Do(call => bytesReceived = call.Arg<byte[]>());

            // Act
            _testClass.Light(sequence, currentElapsed);
            _testClass.Light(sequence, currentElapsed);

            _management.Received(1).Write(Arg.Any<byte[]>());

            // Assert
            bytesReceived.Should().NotBeNull();
            bytesReceived.Should().HaveCount(65);

            bytesReceived[1].Should().Be(16);
            bytesReceived[2].Should().Be(1);
            bytesReceived[3].Should().Be(63);
            bytesReceived[64].Should().Be(80);

            for (int i = 0; i < bytesReceived.Length; i++)
            {
                if (i >= 1 && i <= 3 || i == 64)
                {
                    continue;
                }
                bytesReceived[i].Should().Be(0);
            }
        }

        [Fact]
        public void CannotCallLightWithNullSequence()
        {
            FluentActions.Invoking(() => _testClass.Light(default(Sequence), 1051903891.17)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CanCallDispose()
        {
            // Act
            _testClass.Dispose();

            // Assert
            _management.Received().Dispose();
        }
    }
}