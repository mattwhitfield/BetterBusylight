using System;

namespace BetterBusylight
{
    public interface IBusylightHandler : IDisposable
    {
        void Light(Sequence sequence, double currentElapsed);
    }
}