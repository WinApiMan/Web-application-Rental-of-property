// <copyright file="EmailMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Models
{
    /// <summary>
    /// Class with email fields.
    /// </summary>
    public class EmailMessage
    {
        /// <summary>
        /// Gets or sets email adress.
        /// </summary>
        public string EmailAdress { get; set; }

        /// <summary>
        /// Gets or sets message head.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets email message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets sender name.
        /// </summary>
        public string SenderName { get; set; }
    }
}
