namespace BetterBusylight.Tests
{
    using BetterBusylight;
    using System;
    using Xunit;
    using FluentAssertions;
    using System.IO;
    using System.Drawing;

    public static class ConfigHandlerTests
    {
        [Fact]
        public static void CanCallReadConfig()
        {
            var configPath = Path.Combine(Path.GetDirectoryName(typeof(ConfigHandler).Assembly.Location), "config.xml");
            File.WriteAllText(configPath, TestResources.TestConfig);
            try
            {
                // Act
                var result = ConfigHandler.ReadConfig(out var audioIdentifier);

                // Assert
                result.LockedSequence.FlashFrequency.Should().Be(0);
                result.LockedSequence.Steps.Should().ContainSingle(x => x is Static && ((Static)x).Color.ToArgb() == Color.FromArgb(20, 20, 20).ToArgb());
                result.IdleSequence.FlashFrequency.Should().Be(0);
                result.IdleSequence.Steps.Should().HaveCount(2);
                result.IdleSequence.Steps[0].Should().Match(x => x is Fade && ((Fade)x).StartColor.ToArgb() == Color.FromArgb(0, 128, 0).ToArgb() && ((Fade)x).EndColor.ToArgb() == Color.FromArgb(0, 0, 255).ToArgb());
                result.IdleSequence.Steps[1].Should().Match(x => x is Fade && ((Fade)x).StartColor.ToArgb() == Color.FromArgb(0, 0, 255).ToArgb() && ((Fade)x).EndColor.ToArgb() == Color.FromArgb(0, 128, 0).ToArgb());
                result.AudioOnlySequence.FlashFrequency.Should().Be(0.75);
                result.AudioOnlySequence.Steps.Should().ContainSingle(x => x is Static && ((Static)x).Color.ToArgb() == Color.FromArgb(255, 64, 0).ToArgb());
                result.CameraOnlySequence.FlashFrequency.Should().Be(0.75);
                result.CameraOnlySequence.Steps.Should().ContainSingle(x => x is Static && ((Static)x).Color.ToArgb() == Color.FromArgb(128, 0, 128).ToArgb());
                result.AudioAndCameraSequence.FlashFrequency.Should().Be(1.5);
                result.AudioAndCameraSequence.Steps.Should().ContainSingle(x => x is Static && ((Static)x).Color.ToArgb() == Color.FromArgb(255, 0, 0).ToArgb());
            }
            finally
            {
                File.Delete(configPath);
            }
        }

        [Fact]
        public static void CanCallReadConfigWithNoFile()
        {
            // Act
            var result = ConfigHandler.ReadConfig(out var audioIdentifier);

            // Assert
            result.LockedSequence.FlashFrequency.Should().Be(0);
            result.LockedSequence.Steps.Should().ContainSingle(x => x is Static && ((Static)x).Color.ToArgb() == Color.FromArgb(20, 20, 20).ToArgb());
            result.IdleSequence.FlashFrequency.Should().Be(0);
            result.IdleSequence.Steps.Should().HaveCount(2);
            result.IdleSequence.Steps[0].Should().Match(x => x is Fade && ((Fade)x).StartColor.ToArgb() == Color.FromArgb(0, 128, 0).ToArgb() && ((Fade)x).EndColor.ToArgb() == Color.FromArgb(0, 0, 255).ToArgb());
            result.IdleSequence.Steps[1].Should().Match(x => x is Fade && ((Fade)x).StartColor.ToArgb() == Color.FromArgb(0, 0, 255).ToArgb() && ((Fade)x).EndColor.ToArgb() == Color.FromArgb(0, 128, 0).ToArgb());
            result.AudioOnlySequence.FlashFrequency.Should().Be(0.75);
            result.AudioOnlySequence.Steps.Should().ContainSingle(x => x is Static && ((Static)x).Color.ToArgb() == Color.FromArgb(255, 64, 0).ToArgb());
            result.CameraOnlySequence.FlashFrequency.Should().Be(0.75);
            result.CameraOnlySequence.Steps.Should().ContainSingle(x => x is Static && ((Static)x).Color.ToArgb() == Color.FromArgb(128, 0, 128).ToArgb());
            result.AudioAndCameraSequence.FlashFrequency.Should().Be(1.5);
            result.AudioAndCameraSequence.Steps.Should().ContainSingle(x => x is Static && ((Static)x).Color.ToArgb() == Color.FromArgb(255, 0, 0).ToArgb());
        }
    }
}