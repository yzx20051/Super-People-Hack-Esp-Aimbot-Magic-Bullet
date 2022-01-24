// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FitbitService.cs" company="Magic Bullet Ltd">
//     Copyright (c) Magic Bullet Ltd. All rights reserved.
// </copyright>
// <summary>
//   The fitbit service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MagicBullet.Sample.Forms.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MagicBullet.Sample.Core.Data.Models.External.Fitbit.Activity;
    using MagicBullet.Sample.Core.Data.Models.External.Fitbit.HeartRate;
    using MagicBullet.Sample.Core.Data.Models.External.Fitbit.Sleep;
    using MagicBullet.Sample.Core.Data.Models.External.Fitbit.Weight;
    using MagicBullet.Sample.Core.Helpers.Contants;
    using MagicBullet.Sample.Core.Interfaces;

    using SimpleAuth.Providers;

    /// <summary>
    /// The fitbit service.
    /// </summary>
    public class FitbitService : IFitbitService
    {
        /// <summary>
        /// The breathe services.
        /// </summary>
        private readonly IBreatheServices breatheServices;

        /// <summary>
        /// The api.
        /// </summary>
        private FitBitApi api;

        /// <summary>Initializes a new instance of the <see cref="FitbitService"/> class.</summary>
        /// <param name="breatheServices">The breathe services.</param>
        public FitbitService(IBreatheServices breatheServices)
        {
            this.breatheServices = breatheServices;

            this.api = new BreatheFitBitApi(
                           "fitbit",
                           FitbitSettings.ClientID,
                           FitbitSettings.ClientSecret,
                           false,
                           FitbitSettings.RedirectUrl)
                           {
                               Scopes = FitbitSettings.Scope.Split(' ').ToArray(),
                               AutoAuthenticate = false
                           };
        }

        /// <summary>The get activity data async.</summary>
        /// <param name="token">The token.</param>
        /// <param name="startDate">The start Date.</param>
        /// <param name="endDate">The end Date.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IEnumerable<Activity>> GetActivityDataAsync(string token, DateTime startDate, DateTime endDate)
        {
            var url = string.Format(
                FitbitSettings.FitbitUserCaloriesActivityUrl,
                startDate.ToString("yyyy-MM-dd"),
                endDate.ToString("yyyy-MM-dd"),
                Guid.NewGuid());

            if (!this.api.HasAuthenticated)
            {
                await this.api.Authenticate();
            }

            ActivityCollection activityData = await this.api.Get<ActivityCollection>(url);
            if (activityData.Collection != null && activityData.Collection.Any())
            {
                activityData.Collection = activityData.Collection.Where((Activity x) => x.Calories > 0).ToList();
                return activityData.Collection;
            }

            return new List<Activity>();
        }

        /// <summary>The get heart rate data async.</summary>
        /// <param name="token">The token.</param>
        /// <param name="startDate">The start Date.</param>
        /// <param name="endDate">The end Date.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IEnumerable<HeartRate>> GetHeartRateDataAsync(string token, DateTime startDate, DateTime endDate)
        {
            string url = string.Format(
                FitbitSettings.FitbitUserHeartRate,
                startDate.ToString("yyyy-MM-dd"),
                endDate.ToString("yyyy-MM-dd"),
               Guid.NewGuid());

            if (!this.api.HasAuthenticated)
            {
                await this.api.Authenticate();
            }

            HeartRateCollection heartRateData = await this.api.Get<HeartRateCollection>(url);
            if (heartRateData.Collection != null && heartRateData.Collection.Any())
            {
                heartRateData.Collection = heartRateData.Collection.Where((HeartRate x) => x.Value.RestingHeartRate > 0).ToList();
                return heartRateData.Collection;
            }

            return new List<HeartRate>();
        }

        /// <summary>The get sleep data async.</summary>
        /// <param name="token">The token.</param>
        /// <param name="startDate">The start Date.</param>
        /// <param name="endDate">The end Date.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IEnumerable<Sleep>> GetSleepDataAsync(string token, DateTime startDate, DateTime endDate)
        {
            string url = string.Format(
                FitbitSettings.FitbitUserSleep,
                startDate.ToString("yyyy-MM-dd"),
                endDate.ToString("yyyy-MM-dd"),
                Guid.NewGuid());

            if (!this.api.HasAuthenticated)
            {
                await this.api.Authenticate();
            }

            SleepCollection sleepData = await this.api.Get<SleepCollection>(url);
            if (sleepData.Collection != null && sleepData.Collection.Any())
            {
                sleepData.Collection = sleepData.Collection.Where((Sleep x) => x.MinutesAsleep > 0).ToList();
                return sleepData.Collection;
            }

            return new List<Sleep>();
        }

        /// <summary>The get weight data async.</summary>
        /// <param name="token">The token.</param>
        /// <param name="startDate">The start Date.</param>
        /// <param name="endDate">The end Date.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IEnumerable<Weight>> GetWeightDataAsync(string token, DateTime startDate, DateTime endDate)
        {
            string url = string.Format(
                FitbitSettings.FitbitUserWeightUrl,
                startDate.ToString("yyyy-MM-dd"),
                endDate.ToString("yyyy-MM-dd"),
                Guid.NewGuid());

            try
            {
                if (!this.api.HasAuthenticated)
                {
                    await this.api.Authenticate();
                }

                WeightCollection weightData = await this.api.Get<WeightCollection>(url);
                if (weightData.Collection != null && weightData.Collection.Any())
                {
                    return weightData.Collection;
                }
            }
            catch (Exception e)
            {
            }

            return new List<Weight>();
        }

        /// <summary>
        /// Opens web ui for login and gets an access and refresh token for the user. Storage and refresh is handled by the api.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<bool> LoginAsync()
        {
            try
            {
                await this.api.Authenticate();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Logs the user out of the fitbit service.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Logout()
        {
            try
            {
                this.api?.Logout();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}