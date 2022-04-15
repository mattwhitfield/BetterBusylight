namespace BetterBusylight.Tests
{
    using BetterBusylight;
    using Xunit;
    using FluentAssertions;

    public static class BusylightIdentifierTests
    {
        [Theory]
        [InlineData(10171, 15306, true)]
        [InlineData(10171, 15307, true)]
        [InlineData(10171, 15309, true)]
        [InlineData(1240, 63560, true)]
        [InlineData(10171, 15305, false)]
        [InlineData(10171, 15308, false)]
        [InlineData(10171, 15310, false)]
        [InlineData(10171, 63560, false)]
        [InlineData(1240, 15306, false)]
        [InlineData(1240, 15307, false)]
        [InlineData(1240, 15309, false)]
        public static void CanCallIsBusylight(int vendorId, int productId, bool expected)
        {
            // Act
            var result = BusylightIdentifier.IsBusylight(vendorId, productId);

            // Assert
            result.Should().Be(expected);
        }
    }
}