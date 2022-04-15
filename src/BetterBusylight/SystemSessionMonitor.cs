namespace BetterBusylight
{
    using Microsoft.Win32;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage] // System session locking detection - not testable
    public class SystemSessionMonitor : ISystemSessionMonitor
    {
        public SystemSessionMonitor()
        {
            SystemEvents.SessionSwitch += SystemEventsOnSessionSwitch;
        }

        public bool IsLocked { get; private set; }

        private void SystemEventsOnSessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                IsLocked = true;
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                IsLocked = false;
            }
        }
    }
}
