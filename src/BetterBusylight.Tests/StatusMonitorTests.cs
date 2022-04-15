namespace BetterBusylight.Tests
{
    using BetterBusylight;
    using System;
    using Xunit;
    using FluentAssertions;
    using NSubstitute;
    using System.Drawing;

    public class StatusMonitorTests
    {
        private StatusMonitor _testClass;
        private ISequenceHandler _sequenceHandler;
        private IWebcamHandler _webcamHandler;
        private IBusylightHandler _busylightHandler;
        private IAudioSessionHandler _audioSessionHandler;
        private ISystemSessionMonitor _systemSessionMonitor;

        public StatusMonitorTests()
        {
            _sequenceHandler = Substitute.For<ISequenceHandler>();
            _webcamHandler = Substitute.For<IWebcamHandler>();
            _busylightHandler = Substitute.For<IBusylightHandler>();
            _audioSessionHandler = Substitute.For<IAudioSessionHandler>();
            _systemSessionMonitor = Substitute.For<ISystemSessionMonitor>();
            _testClass = new StatusMonitor(_sequenceHandler, _webcamHandler, _busylightHandler, _audioSessionHandler, _systemSessionMonitor);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new StatusMonitor(_sequenceHandler, _webcamHandler, _busylightHandler, _audioSessionHandler, _systemSessionMonitor);
            
            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CannotConstructWithNullSequenceHandler()
        {
            FluentActions.Invoking(() => new StatusMonitor(default(ISequenceHandler), Substitute.For<IWebcamHandler>(), Substitute.For<IBusylightHandler>(), Substitute.For<IAudioSessionHandler>(), Substitute.For<ISystemSessionMonitor>())).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CannotConstructWithNullWebcamHandler()
        {
            FluentActions.Invoking(() => new StatusMonitor(Substitute.For<ISequenceHandler>(), default(IWebcamHandler), Substitute.For<IBusylightHandler>(), Substitute.For<IAudioSessionHandler>(), Substitute.For<ISystemSessionMonitor>())).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CannotConstructWithNullBusylightHandler()
        {
            FluentActions.Invoking(() => new StatusMonitor(Substitute.For<ISequenceHandler>(), Substitute.For<IWebcamHandler>(), default(IBusylightHandler), Substitute.For<IAudioSessionHandler>(), Substitute.For<ISystemSessionMonitor>())).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CannotConstructWithNullAudioSessionHandler()
        {
            FluentActions.Invoking(() => new StatusMonitor(Substitute.For<ISequenceHandler>(), Substitute.For<IWebcamHandler>(), Substitute.For<IBusylightHandler>(), default(IAudioSessionHandler), Substitute.For<ISystemSessionMonitor>())).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CannotConstructWithNullSystemSessionMonitor()
        {
            FluentActions.Invoking(() => new StatusMonitor(Substitute.For<ISequenceHandler>(), Substitute.For<IWebcamHandler>(), Substitute.For<IBusylightHandler>(), Substitute.For<IAudioSessionHandler>(), default(ISystemSessionMonitor))).Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void CanCallRunWhenNotLocked(bool audio, bool cam)
        {
            // Arrange
            var done = new[] { true, false };
            int i = -1;
            Func<bool> shouldContinue = () => done[++i];

            var sequence = new Sequence(new[] { new Static(Color.Brown), new Static(Color.Yellow), new Static(Color.Green) });
            var lockedSequence = new Sequence(new[] { new Static(Color.Red), new Static(Color.Cyan), new Static(Color.Black) });
            _sequenceHandler.PickSequence(cam, audio).Returns(sequence);
            _sequenceHandler.LockedSequence.Returns(lockedSequence);
            _webcamHandler.IsWebCamInUse().Returns(cam);
            _audioSessionHandler.IsAudioPlaying().Returns(audio);
            _systemSessionMonitor.IsLocked.Returns(false);

            // Act
            _testClass.Run(1, 1, shouldContinue, () => { });

            // Assert
            _sequenceHandler.Received().PickSequence(cam, audio);
            _webcamHandler.Received().IsWebCamInUse();
            _audioSessionHandler.Received().IsAudioPlaying();
            _busylightHandler.Received().Light(sequence, Arg.Any<double>());
        }

        [Fact]
        public void CanCallRunWhenLocked()
        {
            // Arrange
            var done = new[] { true, false };
            int i = -1;
            Func<bool> shouldContinue = () => done[++i];

            var lockedSequence = new Sequence(new[] { new Static(Color.Red), new Static(Color.Cyan), new Static(Color.Black) });
            _sequenceHandler.LockedSequence.Returns(lockedSequence);
            _systemSessionMonitor.IsLocked.Returns(true);

            // Act
            _testClass.Run(1, 1, shouldContinue, () => { });

            // Assert
            _busylightHandler.Received().Light(lockedSequence, Arg.Any<double>());
        }

        [Fact]
        public void CannotCallRunWithRefreshCheckSecondsAndDeviceCheckSectionsAndShouldContinueAndOnLoopWithNullShouldContinue()
        {
            FluentActions.Invoking(() => _testClass.Run(1109321435.97, 1848044727.54, default(Func<bool>), () => { })).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CannotCallRunWithRefreshCheckSecondsAndDeviceCheckSectionsAndShouldContinueAndOnLoopWithNullOnLoop()
        {
            FluentActions.Invoking(() => _testClass.Run(147657528.81, 132365092.86, () => false, default(Action))).Should().Throw<ArgumentNullException>();
        }
    }
}