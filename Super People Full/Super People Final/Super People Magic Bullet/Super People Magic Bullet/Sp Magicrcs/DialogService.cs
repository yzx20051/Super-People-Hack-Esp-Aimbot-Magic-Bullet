//-----------------------------------------------------------------------
// <copyright file="DialogService.cs" company="Magic Bullet Ltd">
//     Copyright (c) Magic Bullet Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MagicBullet.Sample.Forms.Services
{
    using System.Threading;
    using System.Threading.Tasks;

    using MagicBullet.Core.Abstractions;

    /// <summary>
    /// The dialog service.
    /// </summary>
    public class DialogService : IDialogService
    {
        /// <summary>
        /// The alert.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="okText">
        /// The ok text.
        /// </param>
        public void Alert(string message, string title, string okText = "")
        {
            Acr.UserDialogs.UserDialogs.Instance.Alert(message, title, okText);
        }

        /// <summary>
        /// The alert async.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="okText">
        /// The ok text.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task AlertAsync(string message, string title, string okText = "")
        {
            await Acr.UserDialogs.UserDialogs.Instance.AlertAsync(message, title, okText);
        }

        /// <summary>The action sheet async.</summary>
        /// <param name="title">The title.</param>
        /// <param name="cancel">The cancel.</param>
        /// <param name="destructive">The destructive.</param>
        /// <param name="cancelToken">The cancel token.</param>
        /// <param name="buttons">The buttons.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<string> ActionSheetAsync(string title, string cancel, string destructive, CancellationToken? cancelToken = null, params string[] buttons)
        {
            return await Acr.UserDialogs.UserDialogs.Instance.ActionSheetAsync(title, cancel, destructive, cancelToken, buttons);
        }

        /// <summary>
        /// The confirm async.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="okText">
        /// The ok text.
        /// </param>
        /// <param name="cancelText">
        /// The cancel text.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<bool> ConfirmAsync(string title, string message, string okText, string cancelText)
        {
            return await Acr.UserDialogs.UserDialogs.Instance.ConfirmAsync(message, title, okText, cancelText);
        }

        /// <summary>
        /// The toast.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Toast(string message)
        {
            Acr.UserDialogs.UserDialogs.Instance.Toast(message);
        }

        /// <summary>
        /// The hide loading.
        /// </summary>
        public void HideLoading()
        {
            Acr.UserDialogs.UserDialogs.Instance.HideLoading();
        }

        /// <summary>
        /// The show loading.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        public void ShowLoading(string title = null)
        {
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading(title);
        }
    }
}