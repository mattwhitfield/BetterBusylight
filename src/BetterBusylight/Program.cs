namespace BetterBusylight
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Forms;

    [ExcludeFromCodeCoverage] // Main entry point - not testable
    public static class Program
    {
        [MTAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0 && string.Equals(args[0], "list", StringComparison.OrdinalIgnoreCase))
            {
                using (var audioHandler = new AudioSessionHandler("None"))
                {
                    string message = AudioSessionEnumerator.GetAudioDeviceMessage(audioHandler);
                    MessageBox.Show(message, "Audio Devices", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }

            var sequenceHandler = ConfigHandler.ReadConfig(out var audioIdentifier);
            var webcamHandler = new WebcamHandler();
            var sessionMonitor = new SystemSessionMonitor();

            using (var handler = new BusylightHandler(new DeviceManagement()))
            {
                using (var audioHandler = new AudioSessionHandler(audioIdentifier))
                {
                    new StatusMonitor(sequenceHandler, webcamHandler, handler, audioHandler, sessionMonitor).Run();
                }
            }
        }

    }
}
