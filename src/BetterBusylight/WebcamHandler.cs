namespace BetterBusylight
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.Win32;

    [ExcludeFromCodeCoverage] // Registry checks for camera control - not testable
    public class WebcamHandler : IWebcamHandler
    {
        private static readonly string[] RootKeys = new[]
        {
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam",
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam\NonPackaged",
        };

        public bool IsWebCamInUse()
        {
            foreach (var rootKey in RootKeys)
            {
                using (var key = Registry.CurrentUser.OpenSubKey(rootKey))
                {
                    foreach (var subKeyName in key.GetSubKeyNames())
                    {
                        using (var subKey = key.OpenSubKey(subKeyName))
                        {
                            if (subKey.GetValueNames().Contains("LastUsedTimeStop"))
                            {
                                var endTime = subKey.GetValue("LastUsedTimeStop") is long ? (long)subKey.GetValue("LastUsedTimeStop") : -1;
                                if (endTime <= 0)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
