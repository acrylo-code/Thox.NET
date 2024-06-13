using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Http.Headers;
using System.Diagnostics;
using NuGet.Configuration;
using Thox.modules.Mail;

namespace Thox.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            try
            {
                await Mail.SendMail(new Mail.EmailMessage
                {
                    FromAddress = "thox.info@gmail.com",
                    FromName = "Password Recovery",
                    ToAddress = toEmail,
                    ToName = "",
                    Subject = subject,
                    ContentType = Mail.EmailContentType.Text,
                    //PlainTextContent = message,
                    HTMLContent = message

                });
                _logger.Log(LogLevel.Information, "Email sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
            }
        }
    }
}
