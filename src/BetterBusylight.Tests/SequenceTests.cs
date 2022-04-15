namespace BetterBusylight.Tests
{
    using BetterBusylight;
    using System;
    using Xunit;
    using FluentAssertions;
    using NSubstitute;
    using System.Drawing;
    using System.Collections.Generic;

    public class SequenceTests
    {
        private Sequence _testClass;
        private Step[] _steps;
        private double _flashFrequency;

        public SequenceTests()
        {
            _steps = new[] { new Static(Color.Green, 1), new Static(Color.Pink, 1), new Static(Color.Purple, 1) };
            _flashFrequency = 1;
            _testClass = new Sequence(_steps, _flashFrequency);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new Sequence(_steps);
            
            // Assert
            instance.Should().NotBeNull();
            
            // Act
            instance = new Sequence(_steps, _flashFrequency);
            
            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CannotConstructWithNullSteps()
        {
            FluentActions.Invoking(() => new Sequence(default(Step[]))).Should().Throw<ArgumentNullException>();
            FluentActions.Invoking(() => new Sequence(default(IList<Step>), 612209625.39)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CanCallGetColor()
        {
            _testClass.GetColor(0).ToArgb().Should().Be(MultiplyColor(Color.Green, 0.1).ToArgb());
            _testClass.GetColor(0.5).ToArgb().Should().Be(Color.Green.ToArgb());
            _testClass.GetColor(1).ToArgb().Should().Be(MultiplyColor(Color.Pink, 0.1).ToArgb());
            _testClass.GetColor(1.5).ToArgb().Should().Be(Color.Pink.ToArgb());
            _testClass.GetColor(2).ToArgb().Should().Be(MultiplyColor(Color.Purple, 0.1).ToArgb());
            _testClass.GetColor(2.5).ToArgb().Should().Be(Color.Purple.ToArgb());
        }

        private Color MultiplyColor(Color c, double factor)
        {
            return Color.FromArgb((int)(c.R * factor), (int)(c.G * factor), (int)(c.B * factor));
        }

        [Fact]
        public void CanCallToFactor()
        {
            Sequence.ToFactor(0).Should().Be(0.0);
            Sequence.ToFactor(0.5).Should().Be(0.5);
            Sequence.ToFactor(1).Should().Be(1);

            // test it increases over the range
            var current = 0.0;
            for (var i = 0.01; i < 1; i += 0.01)
            {
                var factor = Sequence.ToFactor(i);
                factor.Should().BeGreaterThan(current);
                current = factor;
            }
        }

        [Fact]
        public void StepsIsInitializedCorrectly()
        {
            _testClass = new Sequence(_steps);
            _testClass.Steps.Should().BeSameAs(_steps);
            _testClass = new Sequence(_steps, _flashFrequency);
            _testClass.Steps.Should().BeSameAs(_steps);
        }

        [Fact]
        public void FlashFrequencyIsInitializedCorrectly()
        {
            _testClass.FlashFrequency.Should().Be(_flashFrequency);
        }

        [Fact]
        public void CanSetAndGetFlashFrequency()
        {
            // Arrange
            var testValue = 325589780.34;
            
            // Act
            _testClass.FlashFrequency = testValue;
            
            // Assert
            _testClass.FlashFrequency.Should().Be(testValue);
        }

        [Fact]
        public void CanGetTotalDuration()
        {
            // Assert
            _testClass.TotalDuration.Should().Be(3);
        }
    }
}