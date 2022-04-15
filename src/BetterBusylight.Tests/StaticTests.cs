namespace BetterBusylight.Tests
{
    using BetterBusylight;
    using Xunit;
    using FluentAssertions;
    using System.Drawing;

    public class StaticTests
    {
        private Static _testClass;
        private Color _color;
        private double _duration;

        public StaticTests()
        {
            _color = Color.White;
            _duration = 1699292466.6299999;
            _testClass = new Static(_color, _duration);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new Static(_color);
            
            // Assert
            instance.Should().NotBeNull();
            
            // Act
            instance = new Static(_color, _duration);
            
            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CanCallGetColor()
        {
            // Arrange
            var factor = 1373066471.7;
            
            // Act
            var result = _testClass.GetColor(factor);

            // Assert
            result.Should().Be(_color);
        }

        [Fact]
        public void ColorIsInitializedCorrectly()
        {
            _testClass = new Static(_color);
            _testClass.Color.Should().Be(_color);
            _testClass = new Static(_color, _duration);
            _testClass.Color.Should().Be(_color);
        }
    }
}