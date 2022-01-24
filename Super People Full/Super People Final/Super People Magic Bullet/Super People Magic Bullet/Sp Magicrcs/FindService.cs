//-----------------------------------------------------------------------
// <copyright file="DispatcherService.cs" company="Magic Bullet Ltd">
//     Copyright (c) Magic Bullet Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MagicBullet.Sample.Forms.Services
{
    using System;
    using System.Threading.Tasks;

    using MagicBullet.Core.Abstractions;

    /// <summary>
    /// The dispatcher service.
    /// </summary>
    public class DispatcherService : IDispatcherService
    {
        /// <summary>
        /// The run on ui thread.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        public void RunOnUiThread(Action action)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(action);
        }

        public Task RunOnUiThreadAsync(Action action)
        {
            return Task.Run(() =>
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(action);
                });
        }
    }
}