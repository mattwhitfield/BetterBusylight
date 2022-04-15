namespace BetterBusylight
{
    public interface ISequenceHandler
    {
        Sequence PickSequence(bool webCamActive, bool audioActive);

        Sequence LockedSequence { get; }
    }
}