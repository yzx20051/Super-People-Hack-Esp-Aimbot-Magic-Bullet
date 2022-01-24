// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BreatheFitBitApi.cs" company="Magic Bullet Ltd">
//     Copyright (c) Magic Bullet Ltd. All rights reserved. 
// </copyright>
// <summary>
//   The breathe fit bit api.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MagicBullet.Sample.Forms.Services
{
    using System;
    using System.Threading.Tasks;

    using SimpleAuth;
    using SimpleAuth.Providers;

    /// <summary>The breathe fit bit api.</summary>
    public class BreatheFitBitApi : FitBitApi
    {
        /// <summary>Initializes a new instance of the <see cref="BreatheFitBitApi"/> class.</summary>
        /// <param name="serviceId">The service id.</param>
        /// <param name="clientId">The client id.</param>
        /// <param name="secret">The secret.</param>
        /// <param name="isImplicit">The is implicit.</param>
        /// <param name="redirectUrl">The redirect url.</param>
        public BreatheFitBitApi(
            string serviceId,
            string clientId,
            string secret,
            bool isImplicit,
            string redirectUrl = "http://localhost")
            : base(serviceId, clientId, secret, isImplicit, redirectUrl)
        {
        }

        /// <summary>The refresh account.</summary>
        /// <param name="account">The account.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        protected override Task<bool> RefreshAccount(Account account)
        {
            var authHeader = Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes($"{this.ClientId}:{this.ClientSecret}"));
            this.Client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader);
            return base.RefreshAccount(account);
        }
    }
}