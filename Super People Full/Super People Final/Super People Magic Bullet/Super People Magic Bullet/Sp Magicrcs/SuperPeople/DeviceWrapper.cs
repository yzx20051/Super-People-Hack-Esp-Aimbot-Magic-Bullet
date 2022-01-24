// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeviceWrapper.cs" company="Magic Bullet Ltd">
//     Copyright (c) Magic Bullet Ltd. All rights reserved.
// </copyright>
// <summary>
//   Wraps the native Bluetooth device.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MagicBullet.Sample.Forms.Services.Bluetooth
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    using MagicBullet.Sample.Core.Interfaces.Bluetooth;
    using MagicBullet.Sample.Forms.Helpers.Bluetooth;

    using Plugin.BluetoothLE;

    using ConnectionStatus = MagicBullet.Sample.Core.Enum.Bluetooth.ConnectionStatus;

    /// <summary> Wraps the native Bluetooth device.</summary>
    public class DeviceWrapper : IDeviceWrapper, IDisposable
    {
        /// <summary>The characteristics.</summary>
        private IDisposable characteristics;

        /// <summary>The native device interface.</summary>
        private IDevice device;

        /// <summary>Disposable reference for the  device status.</summary>
        private IDisposable deviceStatus;

        /// <summary>Initializes a new instance of the <see cref="DeviceWrapper"/> class.</summary>
        /// <param name="device">The native device interface.</param>
        /// <param name="hookEvents">Whether or not to hook up events. True to hookup, false to not.</param>
        public DeviceWrapper(IDevice device, bool hookEvents = true)
        {
            this.Setup(device, hookEvents);
        }

        /// <summary>The characteristic found event handler.</summary>
        public event EventHandler<IGattCharacteristicWrapper> CharacteristicFound;

        /// <summary>The status changed event handler.</summary>
        public event EventHandler<ConnectionStatus> StatusChanged;

        /// <summary>Gets the gatt characteristics.</summary>
        public IList<IGattCharacteristicWrapper> GattCharacteristics { get; } = new List<IGattCharacteristicWrapper>();

        /// <summary>Gets the name of the device.</summary>
        public string Name { get; private set; }

        /// <summary>Gets the connection status of the device.</summary>
        public ConnectionStatus Status { get; private set; }

        /// <summary>Gets the uuid. This is the device unique identifier.</summary>
        public Guid Uuid { get; private set; }

        /// <summary>Connect to the bluetooth device.</summary>
        public void Connect()
        {
            this.DeviceStatus_Connect();

            this.device.Connect(
                new ConnectionConfig { AndroidConnectionPriority = ConnectionPriority.High, AutoConnect = false });
        }

        /// <summary>The connect async.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task ConnectAsync()
        {
            this.DeviceStatus_Connect();

            this.device.Connect(
                new ConnectionConfig { AndroidConnectionPriority = ConnectionPriority.High, AutoConnect = false });

            // HACK Delay added because android bluetooth needs time between calls
            await Task.Delay(750);

            await this.device.WhenConnected().Take(1).Timeout(TimeSpan.FromSeconds(10));
        }

        /// <summary>Disconnect to the bluetooth device.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task DisconnectAsync()
        {
            this.device.CancelConnection();

            // HACK Delay added because android bluetooth needs time between calls
            await Task.Delay(750);

            await this.device.WhenDisconnected().Timeout(TimeSpan.FromSeconds(1)).Take(1);

            this.DeviceStatus_Disconnect();
        }

        /// <summary>Disposes the wrapper and all it's event handlers.</summary>
        public void Dispose()
        {
            this.DeviceStatus_Disconnect();
            this.characteristics?.Dispose();
            this.characteristics = null;
        }

        /// <summary>Gets a known characteristic. Can throw exceptions randomly.</summary>
        /// <param name="serviceUuid">The service uuid.</param>
        /// <param name="characteristicUuid">The characteristic uuid.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IGattCharacteristicWrapper> GetKnownCharacteristic(Guid serviceUuid, Guid characteristicUuid)
        {
            try
            {
                var nativeCharacteristic =
                await this.device
                    .GetKnownCharacteristics(serviceUuid, characteristicUuid)
                    .Take(1)
                    .Timeout(TimeSpan.FromSeconds(10));

                return new GattCharacteristicWrapper(nativeCharacteristic);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return null;
            }
        }

        /// <summary>Sets up the device and it's event handlers if needed.</summary>
        /// <param name="newDevice">The native device interfaces.</param>
        /// <param name="hookEvents">Whether or not to hook up events. True to hookup, false to not.</param>
        private void Setup(IDevice newDevice, bool hookEvents)
        {
            try
            {
                this.device = newDevice;
                this.Name = this.device.Name;
                this.Uuid = this.device.Uuid;
                this.Status = this.device.Status.ToConnectionStatus();

                if (hookEvents)
                {
                  this.DeviceStatus_Connect();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"DeviceWrapper - Setup - Error: {e}");
            }
        }

        private void DeviceStatus_Connect()
        {
            this.deviceStatus = this.device.WhenStatusChanged().Subscribe(
                status =>
                    {
                        if (this.Status != status.ToConnectionStatus())
                        {
                            this.Status = status.ToConnectionStatus();
                            this.StatusChanged?.Invoke(this, this.Status);
                        }

                        Console.WriteLine($"DeviceWrapper - WhenStatusChanged - Status: {status.ToString()}");
                    });
        }

        private void DeviceStatus_Disconnect()
        {
            this.deviceStatus?.Dispose();
            this.deviceStatus = null;
        }
    }
}