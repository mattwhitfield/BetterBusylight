namespace BetterBusylight.Tests
{
    using BetterBusylight;
    using System;
    using Xunit;
    using FluentAssertions;

    public class TimedEventTests
    {
        private TimedEvent _testClass;
        private double _elapsed;

        public TimedEventTests()
        {
            _elapsed = 1;
            _testClass = new TimedEvent(_elapsed);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new TimedEvent(_elapsed);
            
            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CanCallIsDue()
        {
            // initial tick
            _testClass.IsDue(0).Should().BeTrue();

            // not due
            _testClass.IsDue(0.1).Should().BeFalse();

            // due
            _testClass.IsDue(1.0).Should().BeTrue();

            // not due
            _testClass.IsDue(1.1).Should().BeFalse();
        }

        [Fact]
        public void CanCallIsDueWithLargeStep()
        {
            // initial tick
            _testClass.IsDue(0).Should().BeTrue();

            // not due
            _testClass.IsDue(0.1).Should().BeFalse();

            // due
            _testClass.IsDue(100).Should().BeTrue();
        }
    }
}