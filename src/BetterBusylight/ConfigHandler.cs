using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BetterBusylight
{
    public static class ConfigHandler
    {
        public static SequenceHandler ReadConfig(out string audioIdentifier)
        {
            var configPath = Path.Combine(Path.GetDirectoryName(typeof(ConfigHandler).Assembly.Location), "config.xml");
            if (File.Exists(configPath))
            {
                try
                {
                    return ConfigReader.ReadFrom(configPath, out audioIdentifier);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Could not read configuration - a default configuration will be used.\r\n\r\nError:\r\n" + exc.Message, "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // return a default config
            var idleSequence = new Sequence(new Fade(Color.Green, Color.Blue, 1.5), new Fade(Color.Blue, Color.Green, 1.5));
            var audioAndCamSequence = new Sequence(new Static(Color.Red)) { FlashFrequency = 1.5 };
            var audioSequence = new Sequence(new Static(Color.FromArgb(255, 64, 0))) { FlashFrequency = 0.75 };
            var camSequence = new Sequence(new Static(Color.Purple)) { FlashFrequency = 0.75 };
            var lockedSequence = new Sequence(new Static(Color.FromArgb(20, 20, 20)));

            audioIdentifier = "Jabra";
            return new SequenceHandler(lockedSequence, idleSequence, audioSequence, camSequence, audioAndCamSequence);
        }
    }
}
