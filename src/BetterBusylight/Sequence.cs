namespace BetterBusylight
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    public class Sequence
    {
        public Sequence(params Step[] steps)
            : this(steps, 0)
        { }

        public Sequence(IList<Step> steps, double flashFrequency)
        {
            Steps = steps;
            FlashFrequency = flashFrequency;
            TotalDuration = steps.Sum(x => x.Duration);
        }

        public IList<Step> Steps { get; }
        public double FlashFrequency { get; set; }
        public double TotalDuration { get; }

        public Color GetColor(double currentElapsed)
        {
            var baseColor = GetBaseColor(currentElapsed);

            if (FlashFrequency > 0)
            {
                var cycle = 1 / FlashFrequency;
                // this goes from 0 -> 1 and back to 0 every FlashFrequencyHz
                var cycleFactor = ToFactor((currentElapsed % cycle) / (cycle * 0.5));
                var factor = cycleFactor * 0.9 + 0.1;

                return Color.FromArgb((int)(baseColor.R * factor), (int)(baseColor.G * factor), (int)(baseColor.B * factor));
            }

            return baseColor;
        }

        private Color GetBaseColor(double currentElapsed)
        {
            var ranged = currentElapsed % TotalDuration;
            foreach (var step in Steps)
            {
                if (ranged < step.Duration)
                {
                    return step.GetColor(ToFactor(ranged / step.Duration));
                }
                ranged -= step.Duration;
            }

            return Color.Black;
        }

        public static double ToFactor(double phase)
        {
            return 1 - ((Math.Cos(phase * Math.PI) + 1) / 2);
        }
    }
}
