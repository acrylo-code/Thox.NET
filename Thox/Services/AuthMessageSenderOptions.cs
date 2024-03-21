using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace Thox.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;
        private String _apiKey = Main.GetApiKey("SendGrid_ApiKey");

        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            try
            {
                if (string.IsNullOrEmpty(_apiKey))
                {
                    _apiKey = Main.GetApiKey("SendGrid_ApiKey");
                }
                await Mail.Mail.SendMail(new Mail.Mail.EmailMessage
                {
                    FromAddress = "thox.info@gmail.com",
                    FromName = "Password Recovery",
                    ToAddress = toEmail,
                    ToName = "",
                    Subject = subject,
                    ContentType = Mail.Mail.EmailContentType.Text,
                    //PlainTextContent = message,
                    HTMLContent = message

                });
                Console.WriteLine("Email sent successfully");
                Debug.WriteLine("Email sent successfully" + message);
                //await Execute(_apiKey, subject, message, toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
            }
        }
    }
}
