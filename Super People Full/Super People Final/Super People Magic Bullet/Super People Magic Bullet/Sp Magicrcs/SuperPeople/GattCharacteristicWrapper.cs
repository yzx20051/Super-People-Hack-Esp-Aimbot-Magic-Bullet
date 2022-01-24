// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GattCharacteristicWrapper.cs" company="Magic Bullet Ltd">
//     Copyright (c) Magic Bullet Ltd. All rights reserved.
// </copyright>
// <summary>
//   The gatt characteristic wrapper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MagicBullet.Sample.Forms.Services.Bluetooth
{
    using System;
    using System.Reactive.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using MagicBullet.Sample.Core.Interfaces.Bluetooth;

    using Plugin.BluetoothLE;

    /// <summary>The gatt characteristic wrapper.</summary>
    public class GattCharacteristicWrapper : IGattCharacteristicWrapper, IDisposable
    {
        /// <summary>The notify.</summary>
        private IDisposable notify;

        /// <summary>Initializes a new instance of the <see cref="GattCharacteristicWrapper"/> class.</summary>
        /// <param name="characteristic">The characteristic.</param>
        public GattCharacteristicWrapper(IGattCharacteristic characteristic)
        {
            this.Setup(characteristic);
        }

        /// <summary>The value updated.</summary>
        public event EventHandler<byte[]> ValueUpdated;

        /// <summary>Gets the description.</summary>
        public string Description { get; private set; }

        /// <summary>Gets a value indicating whether is notifying.</summary>
        public bool IsNotifying { get; private set; }

        /// <summary>Gets the uuid.</summary>
        public Guid Uuid { get; private set; }

        /// <summary>Gets or sets the value.</summary>
        public string Value { get; set; }

        /// <summary>Gets or sets the characteristic.</summary>
        private IGattCharacteristic Characteristic { get; set; }

        /// <summary>The dispose.</summary>
        public void Dispose()
        {
            Console.WriteLine(
                $"Gatt Characteristic - Dispose - Uuid:{this.Uuid.ToString()}, IsNotifying:{this.Characteristic.IsNotifying}");
            this.notify?.Dispose();
            this.Characteristic = null;
        }

        /// <summary>The start notify.</summary>
        public void StartNotify()
        {
            Console.WriteLine(
                $"Gatt Characteristic - StartNotify - Uuid:{this.Uuid.ToString()}, IsNotifying:{this.Characteristic.IsNotifying}");

            this.IsNotifying = true;

            this.notify = this.Characteristic.RegisterAndNotify().Subscribe(
                x =>
                    {
                        this.SetReadValue(x, true);
                        this.ValueUpdated?.Invoke(this, x.Data);
                    });
        }

        /// <summary>The stop notify.</summary>
        public void StopNotify()
        {
            Console.WriteLine(
                $"Gatt Characteristic - StopNotify - Uuid:{this.Uuid.ToString()}, IsNotifying:{this.Characteristic.IsNotifying}");

            this.notify?.Dispose();
            this.IsNotifying = false;
        }

        /// <summary>The write.</summary>
        /// <param name="bytes">The bytes.</param>
        public void Write(byte[] bytes)
        {
            Console.WriteLine(
                $"Gatt Characteristic - StopNotify - Uuid:{this.Uuid.ToString()}, CanWrite:{this.Characteristic.CanWrite()}");

            this.Characteristic.Write(bytes).Timeout(TimeSpan.FromSeconds(2)).Subscribe(
                x => Console.WriteLine(
                    $"Gatt Characteristic - Write - Successful: UUID: {this.Uuid.ToString()}"));
        }

        /// <summary>The read async.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<string> ReadAsync()
        {
            if (this.Characteristic.CanRead())
            {
                try
                {
                    var result = await this.Characteristic.Read().Timeout(TimeSpan.FromSeconds(5));
                    this.SetReadValue(result, false);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    this.Value = string.Empty;
                }
            }
            else
            {
                this.Value = string.Empty;
            }

            return this.Value;
        }

        /// <summary>The set read value.</summary>
        /// <param name="result">The result.</param>
        /// <param name="fromUtf8">The from utf 8.</param>
        private void SetReadValue(CharacteristicGattResult result, bool fromUtf8)
        {
            if (result.Data == null)
            {
                this.Value = string.Empty;
            }
            else
            {
                this.Value = fromUtf8
                                 ? Encoding.UTF8.GetString(result.Data, 0, result.Data.Length)
                                 : Encoding.ASCII.GetString(result.Data, 0, result.Data.Length);
            }
        }

        /// <summary>The setup.</summary>
        /// <param name="characteristic">The characteristic.</param>
        private void Setup(IGattCharacteristic characteristic)
        {
            this.Characteristic = characteristic;
            this.Uuid = characteristic.Uuid;
            this.Description = characteristic.Description;
        }
    }
}