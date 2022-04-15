namespace BetterBusylight
{
    using System;

    public class SequenceHandler : ISequenceHandler
    {
        public SequenceHandler(Sequence lockedSequence, Sequence idleSequence, Sequence audioOnlySequence, Sequence cameraOnlySequence, Sequence audioAndCameraSequence)
        {
            LockedSequence = lockedSequence ?? throw new ArgumentNullException(nameof(lockedSequence));
            IdleSequence = idleSequence ?? throw new ArgumentNullException(nameof(idleSequence));
            AudioOnlySequence = audioOnlySequence ?? throw new ArgumentNullException(nameof(audioOnlySequence));
            CameraOnlySequence = cameraOnlySequence ?? throw new ArgumentNullException(nameof(cameraOnlySequence));
            AudioAndCameraSequence = audioAndCameraSequence ?? throw new ArgumentNullException(nameof(audioAndCameraSequence));
        }

        public Sequence PickSequence(bool webCamActive, bool audioActive)
        {
            if (webCamActive)
            {
                return audioActive ? AudioAndCameraSequence : CameraOnlySequence;
            }
            return audioActive ? AudioOnlySequence : IdleSequence;
        }

        public Sequence LockedSequence { get; }
        public Sequence IdleSequence { get; }
        public Sequence AudioOnlySequence { get; }
        public Sequence CameraOnlySequence { get; }
        public Sequence AudioAndCameraSequence { get; }
    }
}
