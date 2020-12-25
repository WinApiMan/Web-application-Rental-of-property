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
    using RentalOfProperty.BusinessLogicLayer.Models;

    /// <summary>
    /// Class for processing email messages.
    /// </summary>
    public class EmailService : IMailService
    {
        /// <summary>
        /// Send message on email.
        /// </summary>
        /// <param name="message">Email message object.</param>
        /// <param name="sendEmailSetting">Email settings.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SendEmailAsync(EmailMessage message, SendEmailSetting sendEmailSetting)
        {
            var emailMessage = new MimeMessage()
            {
                Subject = message.Subject,
                Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message.Message,
                },
            };

            emailMessage.From.Add(new MailboxAddress(message.SenderName, sendEmailSetting.SenderAdress));
            emailMessage.To.Add(new MailboxAddress(string.Empty, message.EmailAdress));

            using var client = new SmtpClient();
            await client.ConnectAsync(sendEmailSetting.Host, sendEmailSetting.Port, (SecureSocketOptions)sendEmailSetting.SocketOptions);
            await client.AuthenticateAsync(sendEmailSetting.SenderAdress, sendEmailSetting.SenderPassword);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
