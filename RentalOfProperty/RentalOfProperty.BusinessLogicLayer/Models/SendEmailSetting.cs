// <copyright file="SendEmailSetting.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Models
{
    using MailKit.Security;

    /// <summary>
    /// Settings in email sender.
    /// </summary>
    public class SendEmailSetting
    {
        /// <summary>
        /// Gets or sets sender email.
        /// </summary>
        public string SenderAdress { get; set; }

        /// <summary>
        /// Gets or sets sender email password.
        /// </summary>
        public string SenderPassword { get; set; }

        /// <summary>
        /// Gets or sets connection host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets connection port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets number of socket option.
        /// </summary>
        public int SocketOptions { get; set; }
    }
}
