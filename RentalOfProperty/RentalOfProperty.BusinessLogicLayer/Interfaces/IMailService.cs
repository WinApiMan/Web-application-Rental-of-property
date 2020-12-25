// <copyright file="IMailService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Interfaces
{
    using System.Threading.Tasks;
    using RentalOfProperty.BusinessLogicLayer.Models;

    /// <summary>
    /// Interface for mail service.
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Send message on email.
        /// </summary>
        /// <param name="message">Email message object.</param>
        /// <param name="sendEmailSetting">Email settings.</param>
        /// <returns>Void return.</returns>
        Task SendEmailAsync(EmailMessage message, SendEmailSetting sendEmailSetting);
    }
}
