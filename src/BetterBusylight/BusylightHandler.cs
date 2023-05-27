namespace BetterBusylight
{
    using System;
    using System.Drawing;

    public sealed class BusylightHandler : IBusylightHandler
    {
        public BusylightHandler(IDeviceManagement management)
        {
            _management = management ?? throw new ArgumentNullException(nameof(management));
        }

        private readonly IDeviceManagement _management;
        private bool _disposed;

        private Color _lastColor = Color.Black;

        TimedEvent _forcedRefresh = new TimedEvent(2.5);

        public void Light(Sequence sequence, double currentElapsed)
        {
            if (sequence is null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }

            try
            {
                var color = sequence.GetColor(currentElapsed);
                var forcedRefreshIsDue = _forcedRefresh.IsDue(currentElapsed);
                if (color == _lastColor && !forcedRefreshIsDue)
                {
                    return;
                }

                _lastColor = color;

                var r = (byte)(color.R * 64 / 256);
                var g = (byte)(color.G * 64 / 256);
                var b = (byte)(color.B * 64 / 256);

                var array = new byte[65];
                array[1] = 16;  // magic value
                array[2] = 1;   // times to repeat
                array[3] = r;
                array[4] = g;
                array[5] = b;

                var total = 17 + r + g + b;
                array[63] = (byte)(total / 256);
                array[64] = (byte)(total % 256);

                _management.Write(array);
            }
            catch (TimeoutException)
            { }
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _management.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
