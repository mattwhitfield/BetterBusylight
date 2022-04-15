namespace BetterBusylight.Tests
{
    using BetterBusylight;
    using System;
    using Xunit;
    using FluentAssertions;
    using System.Drawing;

    public class SequenceHandlerTests
    {
        private SequenceHandler _testClass;
        private Sequence _lockedSequence;
        private Sequence _idleSequence;
        private Sequence _audioOnlySequence;
        private Sequence _cameraOnlySequence;
        private Sequence _audioAndCameraSequence;

        public SequenceHandlerTests()
        {
            _lockedSequence = new Sequence(new[] { new Static(Color.Blue), new Static(Color.Black), new Static(Color.Gold) });
            _idleSequence = new Sequence(new[] { new Static(Color.White), new Static(Color.Cyan), new Static(Color.Brown) });
            _audioOnlySequence = new Sequence(new[] { new Static(Color.Gold), new Static(Color.Magenta), new Static(Color.Yellow) });
            _cameraOnlySequence = new Sequence(new[] { new Static(Color.Gold), new Static(Color.Blue), new Static(Color.Green) });
            _audioAndCameraSequence = new Sequence(new[] { new Static(Color.Orange), new Static(Color.Orange), new Static(Color.Black) });
            _testClass = new SequenceHandler(_lockedSequence, _idleSequence, _audioOnlySequence, _cameraOnlySequence, _audioAndCameraSequence);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new SequenceHandler(_lockedSequence, _idleSequence, _audioOnlySequence, _cameraOnlySequence, _audioAndCameraSequence);
            
            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CannotConstructWithNullLockedSequence()
        {
            FluentActions.Invoking(() => new SequenceHandler(default(Sequence), new Sequence(new[] { new Static(Color.Pink), new Static(Color.Yellow), new Static(Color.Orange) }), new Sequence(new[] { new Static(Color.Lime), new Static(Color.Gold), new Static(Color.White) }), new Sequence(new[] { new Static(Color.Yellow), new Static(Color.Yellow), new Static(Color.Red) }), new Sequence(new[] { new Static(Color.Blue), new Static(Color.Purple), new Static(Color.Green) }))).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CannotConstructWithNullIdleSequence()
        {
            FluentActions.Invoking(() => new SequenceHandler(new Sequence(new[] { new Static(Color.Cyan), new Static(Color.Blue), new Static(Color.Cyan) }), default(Sequence), new Sequence(new[] { new Static(Color.Teal), new Static(Color.White), new Static(Color.Green) }), new Sequence(new[] { new Static(Color.Purple), new Static(Color.Teal), new Static(Color.Red) }), new Sequence(new[] { new Static(Color.White), new Static(Color.Black), new Static(Color.Gold) }))).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CannotConstructWithNullAudioOnlySequence()
        {
            FluentActions.Invoking(() => new SequenceHandler(new Sequence(new[] { new Static(Color.Cyan), new Static(Color.Pink), new Static(Color.White) }), new Sequence(new[] { new Static(Color.Brown), new Static(Color.Orange), new Static(Color.Orange) }), default(Sequence), new Sequence(new[] { new Static(Color.Yellow), new Static(Color.Orange), new Static(Color.Black) }), new Sequence(new[] { new Static(Color.Orange), new Static(Color.Lime), new Static(Color.Pink) }))).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CannotConstructWithNullCameraOnlySequence()
        {
            FluentActions.Invoking(() => new SequenceHandler(new Sequence(new[] { new Static(Color.Lime), new Static(Color.Black), new Static(Color.Magenta) }), new Sequence(new[] { new Static(Color.Blue), new Static(Color.Yellow), new Static(Color.Blue) }), new Sequence(new[] { new Static(Color.Magenta), new Static(Color.Black), new Static(Color.Yellow) }), default(Sequence), new Sequence(new[] { new Static(Color.Blue), new Static(Color.Orange), new Static(Color.Blue) }))).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CannotConstructWithNullAudioAndCameraSequence()
        {
            FluentActions.Invoking(() => new SequenceHandler(new Sequence(new[] { new Static(Color.Magenta), new Static(Color.Teal), new Static(Color.Brown) }), new Sequence(new[] { new Static(Color.Lime), new Static(Color.White), new Static(Color.Blue) }), new Sequence(new[] { new Static(Color.Purple), new Static(Color.Orange), new Static(Color.White) }), new Sequence(new[] { new Static(Color.Black), new Static(Color.Black), new Static(Color.White) }), default(Sequence))).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CanCallPickSequence()
        {
            // Assert
            _testClass.PickSequence(false, false).Should().BeSameAs(_idleSequence);
            _testClass.PickSequence(true, false).Should().BeSameAs(_cameraOnlySequence);
            _testClass.PickSequence(false, true).Should().BeSameAs(_audioOnlySequence);
            _testClass.PickSequence(true, true).Should().BeSameAs(_audioAndCameraSequence);
        }

        [Fact]
        public void LockedSequenceIsInitializedCorrectly()
        {
            _testClass.LockedSequence.Should().BeSameAs(_lockedSequence);
        }

        [Fact]
        public void IdleSequenceIsInitializedCorrectly()
        {
            _testClass.IdleSequence.Should().BeSameAs(_idleSequence);
        }

        [Fact]
        public void AudioOnlySequenceIsInitializedCorrectly()
        {
            _testClass.AudioOnlySequence.Should().BeSameAs(_audioOnlySequence);
        }

        [Fact]
        public void CameraOnlySequenceIsInitializedCorrectly()
        {
            _testClass.CameraOnlySequence.Should().BeSameAs(_cameraOnlySequence);
        }

        [Fact]
        public void AudioAndCameraSequenceIsInitializedCorrectly()
        {
            _testClass.AudioAndCameraSequence.Should().BeSameAs(_audioAndCameraSequence);
        }
    }
}