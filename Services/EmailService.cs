using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using otel_advisor_webApp.Interfaces;

namespace otel_advisor_webApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var mailMessage = new MailMessage("metapersona.agency@gmail.com", to, subject, body)
            {
                IsBodyHtml = true
            };

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
