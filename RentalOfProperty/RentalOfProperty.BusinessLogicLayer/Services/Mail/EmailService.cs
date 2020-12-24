// <copyright file="EmailService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Services.Mail
{
    using System.Threading.Tasks;
    using MailKit.Net.Smtp;
    using MailKit.Security;
    using MimeKit;
    using RentalOfProperty.BusinessLogicLayer.Interfaces;

    /// <summary>
    /// Class for processing email messages.
    /// </summary>
    public class EmailService : IMailService
    {
        /// <summary>
        /// Send message on email.
        /// </summary>
        /// <param name="email">Address of the recipient.</param>
        /// <param name="subject">Message head.</param>
        /// <param name="message">Email message.</param>
        /// <param name="sender">Sender(who send message).</param>
        /// <returns>Void return.</returns>
        public async Task SendEmailAsync(string email, string subject, string message, string sender)
        {
            var emailMessage = new MimeMessage()
            {
                Subject = subject,
                Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message,
                },
            };

            emailMessage.From.Add(new MailboxAddress(sender, "RentalOfProperty@yandex.by"));
            emailMessage.To.Add(new MailboxAddress(string.Empty, email));

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("winapimen@gmail.com", "47Xromosom");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
