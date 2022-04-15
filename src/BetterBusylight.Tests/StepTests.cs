namespace BetterBusylight.Tests
{
    using BetterBusylight;
    using Xunit;
    using FluentAssertions;
    using System.Drawing;

    public class StepTests
    {
        private class TestStep : Step
        {
            public TestStep(double duration) : base(duration)
            {
            }

            public override Color GetColor(double factor)
            {
                return default(Color);
            }
        }

        private TestStep _testClass;
        private double _duration;

        public StepTests()
        {
            _duration = 1089887963.67;
            _testClass = new TestStep(_duration);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new TestStep(_duration);
            
            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void DurationIsInitializedCorrectly()
        {
            _testClass.Duration.Should().Be(_duration);
        }
    }
}