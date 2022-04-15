namespace BetterBusylight
{
    using System.Drawing;

    public class Static : Step
    {
        public Color Color { get; }

        public Static(Color color)
            : this(color, 1)
        { }

        public Static(Color color, double duration)
            : base(duration)
        {
            Color = color;
        }

        public override Color GetColor(double factor)
        {
            return Color;
        }
    }
}
