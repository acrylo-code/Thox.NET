using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using SQLitePCL;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Thox.Mail;

namespace Thox.Controllers
{
    //[Route("api/forms/contact")]
    [ApiController]
    public class APIController : ControllerBase
    {

        [HttpPost("api/forms/contact")]
        public async Task<IActionResult> SubmitContactForm([FromBody] FormData formData)
        {
            // Define field names and corresponding regex patterns
            Dictionary<string, string> fieldRegexMap = new Dictionary<string, string>
            {
                { "Email", @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$" },
                { "Phone", @"^[0-9]{10}$" },
                { "FirstName", @"^(?:[\u00C0-\u01BF\u01C4-\u024F\u1E00-\u1EFFa-zA-Z'’-]+\s?)+$" },
                { "LastName", @"^(?:[\u00C0-\u01BF\u01C4-\u024F\u1E00-\u1EFFa-zA-Z'’-]+\s?)+$" },
                { "Message", @"^(?:[\u00C0-\u01BF\u01C4-\u024F\u1E00-\u1EFFa-zA-Z0-9'’!?@&-]+\s?)+$" }
            };

            var checkDataResult = Validation.checkData(fieldRegexMap, formData);
            if (checkDataResult != null)
            {
                return checkDataResult;
            }

            // Verify reCAPTCHA
            string recaptchaToken = formData.RecaptchaToken;
            bool isRecaptchaValid = await IsRecaptchaValid(recaptchaToken);
            if (!isRecaptchaValid)
            {
                return BadRequest(new { status = "failed", error = "Invalid reCAPTCHA." });
            }
            try
            {

                var mailStatus = Mail.Mail.SendMail(new Mail.Mail.EmailMessage
                {
                    ContentType = Mail.Mail.EmailContentType.ContactForm,
                    FromAddress = "thox.info@gmail.com",
                    FromName = $"{formData.FirstName} {formData.LastName}",
                    ToAddress = "henk.acrylonitril@gmail.com",
                    ToName = "Thox",
                    Subject = "New Contact Form Submission",
                    PlainTextContent = $"You have received a new contact form submission from {formData.FirstName} {formData.LastName}. \nContact info: \n Email: {formData.Email} \n Tel: {formData.Phone}. \n Message: {formData.Message}",
                    TemplateData = new Dictionary<string, string>
                    {
                        { "name", formData.FirstName + " " + formData.LastName },
                        { "email", formData.Email },
                        { "phone", formData.Phone },
                        { "message", formData.Message }
                    }
                }).Result;
                if (mailStatus == "success")
                {
                    return Ok(new { status = "success", message = "Email sent successfully." });
                }
                else
                {
                    return StatusCode(500, new { status = "failed", error = mailStatus });
                }
            }
            catch (Exception ex)
            {
                // Return the error message
                return StatusCode(500, new { status = "failed", error = "Internal server error" });
            }
        }

        private async Task<bool> IsRecaptchaValid(string token)
		{
			try
			{
                if (string.IsNullOrEmpty(token))
                {
                    return false;
                }

				var client = new HttpClient();
				var parameters = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("secret", Main.GetApiKey("Recapcha_Token")),
					new KeyValuePair<string, string>("response", token)
				});

				var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", parameters);
				response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                // Parse the response from Google
                dynamic responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);

                if (responseObject.success != "true")
                {
                    return false;
                }

                // Check if the reCAPTCHA score is above a certain threshold
                double scoreThreshold = 0.5; // Example threshold
                double recaptchaScore = responseObject.score;
                return recaptchaScore >= scoreThreshold;
            }
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"Error validating reCAPTCHA token: {ex.Message}");
                return false;
			}
		}
    }

public static class Validation
    {
        public static BadRequestObjectResult checkData(Dictionary<string, string> fieldRegexMap, object formData)
        {
            // Check if form data is null
            if (formData == null)
            {
                return new BadRequestObjectResult(new { status = "failed", error = "Invalid form data." });
            }

            foreach (var fieldName in fieldRegexMap.Keys)
            {
                var fieldValue = formData.GetType().GetProperty(fieldName)?.GetValue(formData, null)?.ToString();

                // Check if the field is empty
                if (string.IsNullOrEmpty(fieldValue))
                {
                    return new BadRequestObjectResult(new { status = "failed", error = $"{fieldName} is required." });
                }

                // Check if the field value matches the regex pattern
                if (!Regex.IsMatch(fieldValue, fieldRegexMap[fieldName]))
                {
                    return new BadRequestObjectResult(new { status = "failed", error = $"Invalid {fieldName.ToLower()} format." });
                }
            }

            // If all validations pass, return null
            return null;
        }
    }


    public class FormData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
		public string RecaptchaToken { get; set; }
	}
}
