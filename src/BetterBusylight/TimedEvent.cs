namespace BetterBusylight
{
    public class TimedEvent
    {
        private double _elapsed;
        private double _last = double.MinValue;

        public TimedEvent(double elapsed)
        {
            _elapsed = elapsed;
        }

        public bool IsDue(double currentElapsed)
        {
            if (currentElapsed >= _last + _elapsed)
            {
                _last = currentElapsed;
                return true;
            }

            return false;
        }
    }
}
