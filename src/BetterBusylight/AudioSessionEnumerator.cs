namespace BetterBusylight
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class AudioSessionEnumerator
    {
        public static string GetAudioDeviceMessage(IAudioSessionHandler audioHandler)
        {
            if (audioHandler is null)
            {
                throw new ArgumentNullException(nameof(audioHandler));
            }

            var active = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var other = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            audioHandler.EnumerateAudioDevices(active, other);

            var activePrompt = active.Count > 0 ? string.Join("\r\n", active.OrderBy(x => x)) : "None";
            var otherPrompt = other.Count > 0 ? string.Join("\r\n", other.OrderBy(x => x)) : "None";

            return "The available audio devices are:\r\n\r\nActive:\r\n" + activePrompt + "\r\n\r\nOther:\r\n" + otherPrompt;
        }
    }
}
