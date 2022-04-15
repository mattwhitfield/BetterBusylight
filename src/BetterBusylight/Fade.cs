namespace BetterBusylight
{
    using System.Drawing;

    public class Fade : Step
    {
        public Fade(Color startColor, Color endColor, double duration)
            : base(duration)
        {
            StartColor = startColor;
            EndColor = endColor;
        }

        public Color StartColor { get; }

        public Color EndColor { get; }

        public override Color GetColor(double factor)
        {
            var rDiff = EndColor.R - StartColor.R;
            var gDiff = EndColor.G - StartColor.G;
            var bDiff = EndColor.B - StartColor.B;

            return Color.FromArgb(StartColor.R + (int)(factor * rDiff), StartColor.G + (int)(factor * gDiff), StartColor.B + (int)(factor * bDiff));
        }
    }
}
