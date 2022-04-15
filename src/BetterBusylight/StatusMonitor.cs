using System;
using System.Threading;

namespace BetterBusylight
{
    public class StatusMonitor
    {
        private readonly ISequenceHandler _sequenceHandler;
        private readonly IWebcamHandler _webcamHandler;
        private readonly IBusylightHandler _busylightHandler;
        private readonly IAudioSessionHandler _audioSessionHandler;
        private readonly ISystemSessionMonitor _systemSessionMonitor;

        public StatusMonitor(ISequenceHandler sequenceHandler, IWebcamHandler webcamHandler, IBusylightHandler busylightHandler, IAudioSessionHandler audioSessionHandler, ISystemSessionMonitor systemSessionMonitor)
        {
            _sequenceHandler = sequenceHandler ?? throw new ArgumentNullException(nameof(sequenceHandler));
            _webcamHandler = webcamHandler ?? throw new ArgumentNullException(nameof(webcamHandler));
            _busylightHandler = busylightHandler ?? throw new ArgumentNullException(nameof(busylightHandler));
            _audioSessionHandler = audioSessionHandler ?? throw new ArgumentNullException(nameof(audioSessionHandler));
            _systemSessionMonitor = systemSessionMonitor ?? throw new ArgumentNullException(nameof(systemSessionMonitor));
        }

        public void Run()
        {
            Run(1, 2, () => true, () => Thread.Sleep(50));
        }

        public void Run(double refreshCheckSeconds, double deviceCheckSections, Func<bool> shouldContinue, Action onLoop)
        {
            if (shouldContinue is null)
            {
                throw new ArgumentNullException(nameof(shouldContinue));
            }

            if (onLoop is null)
            {
                throw new ArgumentNullException(nameof(onLoop));
            }

            TimedEvent lockedRefresh = new TimedEvent(refreshCheckSeconds), deviceUsageCheck = new TimedEvent(deviceCheckSections);
            bool audioInUse = false, webCamInUse = false;
            var startTime = DateTime.UtcNow;

            while (shouldContinue())
            {
                var currentElapsed = (DateTime.UtcNow - startTime).TotalSeconds;

                if (_systemSessionMonitor.IsLocked)
                {
                    if (lockedRefresh.IsDue(currentElapsed))
                    {
                        _busylightHandler.Light(_sequenceHandler.LockedSequence, currentElapsed);
                    }
                }
                else
                {
                    if (deviceUsageCheck.IsDue(currentElapsed))
                    {
                        webCamInUse = _webcamHandler.IsWebCamInUse();
                        audioInUse = _audioSessionHandler.IsAudioPlaying();
                    }

                    var sequence = _sequenceHandler.PickSequence(webCamInUse, audioInUse);
                    _busylightHandler.Light(sequence, currentElapsed);
                }

                onLoop();
            }
        }
    }
}
