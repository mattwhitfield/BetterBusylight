namespace BetterBusylight.Tests
{
    using BetterBusylight;
    using System;
    using Xunit;
    using FluentAssertions;
    using System.Drawing;

    public class FadeTests
    {
        private Fade _testClass;
        private Color _startColor;
        private Color _endColor;
        private double _duration;

        public FadeTests()
        {
            _startColor = Color.FromArgb(255, 0, 0);
            _endColor = Color.FromArgb(0, 255, 0);
            _duration = 641144938.5;
            _testClass = new Fade(_startColor, _endColor, _duration);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new Fade(_startColor, _endColor, _duration);
            
            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CanCallGetColor()
        {
            _testClass.GetColor(0).ToArgb().Should().Be(Color.FromArgb(255, 0, 0).ToArgb());
            _testClass.GetColor(0.5).ToArgb().Should().Be(Color.FromArgb(128, 127, 0).ToArgb());
            _testClass.GetColor(1).ToArgb().Should().Be(Color.FromArgb(0, 255, 0).ToArgb());
        }

        [Fact]
        public void StartColorIsInitializedCorrectly()
        {
            _testClass.StartColor.Should().Be(_startColor);
        }

        [Fact]
        public void EndColorIsInitializedCorrectly()
        {
            _testClass.EndColor.Should().Be(_endColor);
        }
    }
}