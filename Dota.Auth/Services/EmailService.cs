using System.Net;
using System.Net.Mail;

namespace Dota.Auth.Services
{
    public class EmailService : IEmailService
    {
        private const string MailAddress = "phowar@yandex.ru";
        private readonly SmtpClient _smtpClient = new SmtpClient("smtp.yandex.ru")
        {
            Credentials = new NetworkCredential(MailAddress, "Ph0warthef1rst"),
            Port = 587,
            EnableSsl = true,
        };

        private readonly MailAddress _mailAddressFrom = new MailAddress(MailAddress);

        public void SendEmail(string email, string message)
        {
            var mail = new MailMessage()
            {
                From = _mailAddressFrom,
                To = { new MailAddress(email) },
                Subject = "Email confirmation",
                Body = message,
                IsBodyHtml = true,
            };

            _smtpClient.Send(mail);
        }
    }
}