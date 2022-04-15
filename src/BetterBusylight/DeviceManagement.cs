using HidSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BetterBusylight
{
    [ExcludeFromCodeCoverage] // USB HID device control - not testable
    public class DeviceManagement : IDeviceManagement
    {
        public DeviceManagement()
        {
            DeviceList.Local.Changed += List_Changed;
            Initialize();
        }

        private void List_Changed(object sender, DeviceListChangedEventArgs e)
        {
            Initialize();
        }

        private List<HidStream> _streams = new List<HidStream>();
        private bool _disposed;

        public void Write(byte[] buffer)
        {
            foreach (var stream in _streams)
            {
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        private void Initialize()
        {
            CloseStreams();

            foreach (var device in DeviceList.Local.GetHidDevices())
            {
                if (BusylightIdentifier.IsBusylight(device.VendorID, device.ProductID))
                {
                    if (device.TryOpen(out var stream))
                    {
                        _streams.Add(stream);
                    }
                }
            }
        }

        private void CloseStreams()
        {
            foreach (var stream in _streams)
            {
                stream.Dispose();
            }
            _streams.Clear();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    CloseStreams();
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
