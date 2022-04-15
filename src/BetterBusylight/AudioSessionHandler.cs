namespace BetterBusylight
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using CSCore.CoreAudioAPI;

    [ExcludeFromCodeCoverage] // CsCore makes heavy use of COM - not testable
    public class AudioSessionHandler : IAudioSessionHandler
    {
        public AudioSessionHandler(string identifier)
        {
            Identifier = identifier;
        }

        private MMDeviceEnumerator _enumerator;
        private AudioSessionManager2 _audioSession;
        private MMDevice _currentEndpoint;
        private bool _audioSessionNeedsRecreation;
        private bool _disposed;

        public string Identifier { get; }

        public bool IsAudioPlaying()
        {
            if (_audioSession == null || _audioSessionNeedsRecreation)
            {
                RecreateAudioSession();
            }

            if (_audioSession == null)
            {
                return false;
            }

            var found = false;

            try
            {
                using (var audioSessionEnumerator = _audioSession.GetSessionEnumerator())
                {
                    foreach (var item in audioSessionEnumerator)
                    {
                        using (item)
                        {
                            if (item.SessionState == AudioSessionState.AudioSessionStateActive)
                            {
                                found = true;
                            }
                        }
                    }
                }
            }
            catch (CoreAudioAPIException)
            {
            }

            return found;
        }

        public void EnumerateAudioDevices(ISet<string> activeDevices, ISet<string> otherDevices)
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                using (var enumAudioEndpoints = enumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.All))
                {
                    foreach (var endpoint in enumAudioEndpoints)
                    {
                        if ((endpoint.DeviceState & DeviceState.Active) > 0)
                        {
                            activeDevices.Add(endpoint.FriendlyName);
                        }
                        else
                        {
                            otherDevices.Add(endpoint.FriendlyName);
                        }
                        endpoint.Dispose();
                    }
                }
            }
        }

        private void RecreateAudioSession()
        {
            try
            {
                CloseSessions();

                if (_enumerator == null)
                {
                    _enumerator = new MMDeviceEnumerator();

                    _enumerator.DevicePropertyChanged += FlagRecreateAudioSession;
                    _enumerator.DeviceStateChanged += FlagRecreateAudioSession;
                    _enumerator.DeviceRemoved += FlagRecreateAudioSession;
                    _enumerator.DeviceAdded += FlagRecreateAudioSession;
                }

                if (_currentEndpoint == null || _audioSession == null)
                {
                    using (var enumAudioEndpoints = _enumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
                    {
                        foreach (var endpoint in enumAudioEndpoints)
                        {
                            if (endpoint.FriendlyName.IndexOf(Identifier, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                _currentEndpoint = endpoint;
                                _audioSession = AudioSessionManager2.FromMMDevice(endpoint);
                                _audioSessionNeedsRecreation = false;

                                break;
                            }
                            endpoint.Dispose();
                        }
                    }
                }
            }
            catch (CoreAudioAPIException)
            {
                CloseSessions();
            }
        }

        private void CloseSessions()
        {
            if (_currentEndpoint != null)
            {
                _currentEndpoint.Dispose();
                _currentEndpoint = null;
            }

            if (_audioSession != null)
            {
                _audioSession.Dispose();
                _audioSession = null;
            }

            if (_enumerator != null)
            {
                _enumerator.DevicePropertyChanged -= FlagRecreateAudioSession;
                _enumerator.DeviceStateChanged -= FlagRecreateAudioSession;
                _enumerator.DeviceRemoved -= FlagRecreateAudioSession;
                _enumerator.DeviceAdded -= FlagRecreateAudioSession;

                _enumerator.Dispose();
                _enumerator = null;
            }
        }

        private void FlagRecreateAudioSession(object sender, DeviceNotificationEventArgs e)
        {
            _audioSessionNeedsRecreation = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    CloseSessions();
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
