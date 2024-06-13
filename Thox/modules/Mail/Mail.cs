using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using Microsoft.Identity.Client;
using SendGrid.Helpers.Mail.Model;
using NuGet.Configuration;

namespace Thox.modules.Mail
{
    public class Mail
    {
        private static string sendgrid_APIKEY;

        public class EmailMessage
        {
            public string FromAddress { get; set; }
            public string FromName { get; set; }
            public string ToAddress { get; set; }
            public string ToName { get; set; }
            public string Subject { get; set; }
            public EmailContentType ContentType { get; set; }
            public string? PlainTextContent { get; set; }
            public Dictionary<string, string>? TemplateData { get; set; }

            public string? HTMLContent { get; set; }
        }

        public enum EmailContentType
        {
            Text,
            ContactForm,
            ReservationDetails,
        }
        public static async Task<string> SendMail(EmailMessage emailMessage)
        {
            try
            {
                if (sendgrid_APIKEY == null)
                    sendgrid_APIKEY = Settings.GetApiKey("SendGrid_ApiKey");
                var client = new SendGridClient(sendgrid_APIKEY);
                var from = new EmailAddress(emailMessage.FromAddress, emailMessage.FromName);
                var to = new EmailAddress(emailMessage.ToAddress, emailMessage.ToName);

                // Fetch template based on subject or any other identifier
                string template = GetTemplate(emailMessage.ContentType);

                // Replace placeholders with actual data
                if (emailMessage.TemplateData != null && template != null)
                {
                    foreach (var kvp in emailMessage.TemplateData)
                    {
                        //check if the value exists in the template
                        if (!template.Contains($"{{{{{kvp.Key}}}}}"))
                            continue;
                        template = template.Replace($"{{{{{kvp.Key}}}}}", kvp.Value);
                    }
                }

                string htmlContent = template ?? emailMessage.HTMLContent;

                // Create the email message 
                var msg = MailHelper.CreateSingleEmail(from, to, emailMessage.Subject, emailMessage.PlainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)                // Check the response
                    return "success";
                return response.Body.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private static string GetTemplate(EmailContentType subject)
        {
            if (subject == EmailContentType.Text)
                return null;
            //turn the enum into a string
            subject = (EmailContentType)Enum.Parse(typeof(EmailContentType), subject.ToString());
            Debug.WriteLine(subject);
            //check if the file exists use a relative path
            if (!File.Exists($"Mail/Templates/{subject}.html"))
                return null;


            return File.ReadAllText($"Mail/Templates/{subject}.html");
        }
    }
}