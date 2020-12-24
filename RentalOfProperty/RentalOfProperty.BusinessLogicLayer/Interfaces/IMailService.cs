namespace RentalOfProperty.BusinessLogicLayer.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for mail service.
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Send message on email.
        /// </summary>
        /// <param name="email">Address of the recipient.</param>
        /// <param name="subject">Message head.</param>
        /// <param name="message">Email message.</param>
        /// <param name="sender">Sender(who send message).</param>
        /// <returns>Void return.</returns>
        Task SendEmailAsync(string email, string subject, string message, string sender);
    }
}
