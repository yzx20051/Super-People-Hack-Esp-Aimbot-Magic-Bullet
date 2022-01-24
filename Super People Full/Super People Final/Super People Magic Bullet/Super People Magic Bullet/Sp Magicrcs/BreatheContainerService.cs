// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BreatheContainerService.cs" company="Magic Bullet Ltd">
//     Copyright (c) Magic Bullet Ltd. All rights reserved.
// </copyright>
// <summary>
//   The breathe container service.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace MagicBullet.Sample.Forms.Infrastructure
{
    using FreshMvvm;

    using MagicBullet.Core.Abstractions;
    using MagicBullet.Core.Services;
    using MagicBullet.Sample.Core.Helpers.Contants;
    using MagicBullet.Sample.Core.Interfaces;
    using MagicBullet.Sample.Core.Interfaces.Bluetooth;
    using MagicBullet.Sample.Forms.Services;
    using MagicBullet.Sample.Forms.Services.Bluetooth;
    using MagicBullet.Sample.Forms.ViewModels;

    using Xamarin.Forms;

    /// <summary>
    /// The breathe container service.
    /// </summary>
    public class BreatheContainerService : BaseContainerService
    {
        /// <summary>
        /// The get instance.
        /// </summary>
        /// <typeparam name="TClass">
        /// </typeparam>
        /// <returns>
        /// The <see cref="TClass"/>.
        /// </returns>
        public override TClass GetInstance<TClass>()
        {
            return FreshIOC.Container.Resolve<TClass>();
        }

        /// <summary>
        /// The init.
        /// </summary>
        public override void Init()
        {
            FreshPageModelResolver.PageModelMapper = new BreatheFreshPageMapper();

            this.Register<IDispatcherService, DispatcherService>();
            this.Register<IDialogService, DialogService>();
            this.Register<IFitbitService, FitbitService>();
            this.Register<IBreatheServices, BreatheServices>();
            this.Register<IAdapterWrapper, AdapterWrapper>();
        }

        /// <summary>
        /// The run app launch.
        /// </summary>
        public override void RunAppLaunch()
        {
            Page pageToShow = FreshPageModelResolver.ResolvePageModel<MainViewModel>();
            var mainNavContainer = new FreshNavigationContainer(pageToShow, NavigationContainerNames.MainContainer);
            Application.Current.MainPage = mainNavContainer;
        }

        /// <summary>
        /// The register.
        /// </summary>
        /// <typeparam name="TInterface">
        /// </typeparam>
        /// <typeparam name="TClass">
        /// </typeparam>
        protected override void Register<TInterface, TClass>()
        {
            FreshIOC.Container.Register<TInterface, TClass>();
        }
    }
}