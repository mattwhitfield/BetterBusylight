using System;

namespace BetterBusylight
{
    public interface IDeviceManagement : IDisposable
    {
        void Write(byte[] buffer);
    }
}