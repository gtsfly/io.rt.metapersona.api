using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using otel_advisor_webApp.Interfaces;

namespace otel_advisor_webApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly string _senderEmail;
        private readonly string _senderName;

        public EmailService(SmtpClient smtpClient, IConfiguration configuration, ILogger<EmailService> logger)
        {
            _smtpClient = smtpClient;
            _configuration = configuration;
            _logger = logger;

            var emailSettings = _configuration.GetSection("EmailSettings");
            _senderEmail = emailSettings["SenderEmail"];
            _senderName = emailSettings["SenderName"];

            // SMTP
            _smtpClient.Host = emailSettings["SmtpServer"];
            _smtpClient.Port = int.Parse(emailSettings["SmtpPort"]);
            _smtpClient.EnableSsl = true;
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.Credentials = new NetworkCredential(
                emailSettings["SmtpUser"],
                emailSettings["SmtpPass"]
            );
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                _logger.LogInformation($"Attempting to send email to {to}");

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_senderEmail, _senderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(to);

                await _smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email successfully sent to {to}");
            }
            catch (SmtpException ex)
            {
                _logger.LogError($"SMTP error occurred while sending email: {ex.Message}, Status Code: {ex.StatusCode}");
                throw new Exception($"Email sending failed: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error occurred while sending email: {ex.Message}");
                throw new Exception("An unexpected error occurred while sending the email", ex);
            }
        }
    }
}
