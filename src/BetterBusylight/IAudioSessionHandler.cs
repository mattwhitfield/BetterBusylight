using System;
using System.Collections.Generic;

namespace BetterBusylight
{
    public interface IAudioSessionHandler : IDisposable
    {
        string Identifier { get; }

        bool IsAudioPlaying();

        void EnumerateAudioDevices(ISet<string> activeDevices, ISet<string> otherDevices);
    }
}