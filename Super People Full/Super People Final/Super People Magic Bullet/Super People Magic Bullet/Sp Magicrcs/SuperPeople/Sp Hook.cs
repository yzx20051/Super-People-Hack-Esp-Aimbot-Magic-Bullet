// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdapterWrapper.cs" company="Magic Bullet Ltd">
//     Copyright (c) Magic Bullet Ltd. All rights reserved.
// </copyright>
// <summary>
//   Wraps the native phone bluetooth adapter .
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MagicBullet.Sample.Forms.Services.Bluetooth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    using MagicBullet.Sample.Core.Enum.Bluetooth;
    using MagicBullet.Sample.Core.Interfaces.Bluetooth;
    using MagicBullet.Sample.Forms.Helpers.Bluetooth;

    using Plugin.BluetoothLE;

    using AdapterStatus = MagicBullet.Sample.Core.Enum.Bluetooth.AdapterStatus;

    /// <summary>The adapter wrapper.</summary>
    public class AdapterWrapper : IAdapterWrapper
    {
        /// <summary>The adapter.</summary>
        private IAdapter adapter;

        /// <summary>The scan.</summary>
        private IDisposable scan;

        /// <summary>Initializes a new instance of the <see cref="AdapterWrapper"/> class.</summary>
        public AdapterWrapper()
        {
            this.ConnectAdapter();
        }

        /// <summary>The adapter status changed.</summary>
        public event EventHandler<AdapterStatus> AdapterStatusChanged;

        /// <summary>The scan result updated.</summary>
        public event EventHandler<IEnumerable<IScanResultWrapper>> ScanResultUpdated;

        /// <summary>Gets a value indicating whether the is scanning.</summary>
        public bool IsScanning => this.adapter.IsScanning;

        /// <summary>Gets the status.</summary>
        public AdapterStatus Status { get; private set; }

        /// <summary>The get device async.</summary>
        /// <param name="uuid">The uuid.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IDeviceWrapper> GetDeviceAsync(Guid uuid)
        {
            var pairedDevices = await this.adapter.GetPairedDevices();
            var pairedDevice = pairedDevices.FirstOrDefault(x => x.Uuid == uuid);

            if (pairedDevice != null)
            {
                return new DeviceWrapper(pairedDevice);
            }
            else
            {
                var device = await this.adapter.GetKnownDevice(uuid);
                return new DeviceWrapper(device);
            }
        }

        /// <summary>The start scan.</summary>
        /// <param name="deviceType">The device type.</param>
        public void StartScan(DeviceType deviceType)
        {
            if (!this.IsScanning)
            {
                this.scan = this.adapter
                    .Scan(this.GetScanConfig(deviceType))
                    .Buffer(TimeSpan.FromSeconds(1))
                    .Subscribe(
                    results =>
                        {
                            // Only raise events if there's items to return
                            if (results.Any())
                            {
                                this.ScanResultUpdated?.Invoke(this, results.Select(x => new ScanResultWrapper(x)));
                            }
                        });
            }
        }

        /// <summary>The stop scan.</summary>
        public void StopScan()
        {
            this.scan?.Dispose();
            this.scan = null;
        }

        /// <summary>The connect adapter.</summary>
        private void ConnectAdapter()
        {
            this.adapter = CrossBleAdapter.Current;

            // Initialise the status on first hookup
            this.Status = this.adapter.Status.ToBluetoothAdapterStatus();

            this.adapter.WhenStatusChanged().Subscribe(
                status =>
                    {
                        // Only notify if the status had changed. Stops double power off status events before a reconnect.
                        if (this.Status != status.ToBluetoothAdapterStatus())
                        {
                            this.Status = status.ToBluetoothAdapterStatus();
                            this.AdapterStatusChanged?.Invoke(this, this.Status);
                        }
                    });
        }

        /// <summary>The get scan config.</summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="ScanConfig"/>.</returns>
        private ScanConfig GetScanConfig(DeviceType type)
        {
            ScanConfig config = new ScanConfig() { ScanType = BleScanType.LowLatency };

            switch (type)
            {
                case DeviceType.Spirometer:
                    // device config here
                    break;
                case DeviceType.Oximeter:
                    // device config here
                    break;
                case DeviceType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return config;
        }
    }
}