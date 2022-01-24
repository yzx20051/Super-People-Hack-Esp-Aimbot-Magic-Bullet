// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BreatheServices.cs" company="Magic Bullet Ltd">
//     Copyright (c) Magic Bullet Ltd. All rights reserved.
// </copyright>
// <summary>
//   The breathe services.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace MagicBullet.Sample.Forms.Services
{
    using MagicBullet.Core.Abstractions;
    using MagicBullet.Sample.Core.Interfaces;
    using MagicBullet.Sample.Core.Interfaces.Bluetooth;

    /// <summary>
    /// The breathe services.
    /// </summary>
    public class BreatheServices : IBreatheServices
    {


        /// <summary>
        /// Initializes a new instance of the <see cref="BreatheServices"/> class.
        /// </summary>
        /// <param name="dispatcherService">The dispatcher service.</param>
        /// <param name="dialogService">The dialog service.</param>
        public BreatheServices(
            IDispatcherService dispatcherService,
            IDialogService dialogService,
            IAdapterWrapper bluetoothManager,
            IFitbitService fitbitService)
        {
            this.DispatcherService = dispatcherService;
            this.DialogService = dialogService;
            this.BluetoothManager = bluetoothManager;
            this.FitbitService = fitbitService;
        }

        /// <summary>
        /// Gets the dispatcher service.
        /// </summary>
        public IDispatcherService DispatcherService { get; }

        /// <summary>
        /// Gets the dialog service.
        /// </summary>
        public IDialogService DialogService { get; }

        /// <summary>
        /// Gets the fitbit service.
        /// </summary>
        public IFitbitService FitbitService { get; }

        /// <summary>
        /// Gets the bluetooth manager.
        /// </summary>
        public IAdapterWrapper BluetoothManager { get; }
    }
}