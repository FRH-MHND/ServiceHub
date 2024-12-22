using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ServiceHub.Models;
using ServiceHub.Services.Interfaces;

namespace ServiceHub.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly Email _emailSettings;

        public EmailService(Email emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public async Task SendVerificationCode(string email, string code)
        {
            var smtpClient = new SmtpClient(_emailSettings.SmtpServer)
            {
                Port = _emailSettings.SmtpPort,
                Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPassword),
                EnableSsl = _emailSettings.EnableSsl,
                UseDefaultCredentials = _emailSettings.UseDefaultCredentials
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                Subject = "Your Verification Code",
                Body = $"Your verification code is {code}",
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
