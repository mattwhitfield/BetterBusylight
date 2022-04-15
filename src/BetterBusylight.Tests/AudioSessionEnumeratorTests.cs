namespace BetterBusylight.Tests
{
    using BetterBusylight;
    using System;
    using Xunit;
    using FluentAssertions;
    using NSubstitute;
    using System.Collections.Generic;

    public static class AudioSessionEnumeratorTests
    {
        private class TestAudioHandler : IAudioSessionHandler
        {
            public string Identifier => "i";

            public void Dispose() { }

            public void EnumerateAudioDevices(ISet<string> activeDevices, ISet<string> otherDevices)
            {
                activeDevices.Add("aa");
                activeDevices.Add("bb");
                otherDevices.Add("cc");
                otherDevices.Add("dd");
            }

            public bool IsAudioPlaying() => false;
        }

        [Fact]
        public static void CanCallGetAudioDeviceMessage()
        {
            // Arrange
            var audioHandler = new TestAudioHandler();

            // Act
            var result = AudioSessionEnumerator.GetAudioDeviceMessage(audioHandler);

            // Assert
            result.Should().Be("The available audio devices are:\r\n\r\nActive:\r\naa\r\nbb\r\n\r\nOther:\r\ncc\r\ndd");
        }

        [Fact]
        public static void CannotCallGetAudioDeviceMessageWithNullAudioHandler()
        {
            FluentActions.Invoking(() => AudioSessionEnumerator.GetAudioDeviceMessage(default(IAudioSessionHandler))).Should().Throw<ArgumentNullException>();
        }
    }
}