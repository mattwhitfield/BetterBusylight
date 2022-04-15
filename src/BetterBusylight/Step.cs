namespace BetterBusylight
{
    using System.Drawing;

    public abstract class Step
    {
        public Step(double duration)
        {
            Duration = duration;
        }

        public double Duration { get; }

        public abstract Color GetColor(double factor);
    }
}
