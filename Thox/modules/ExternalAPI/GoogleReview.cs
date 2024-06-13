using System.Diagnostics;
using System.Text.Json;
<<<<<<< HEAD
using Thox.Models.DataModels.Review;
=======
using Thox.Models.DataModels;
>>>>>>> d458c49 (init)
using Thox.modules.webscraper;

namespace Thox.modules.ExternalAPI
{
    public static class GoogleReview
    {
        public static async Task<ExternalReviewModel> GetReviewsGoogle(string apiKey, string placeId)
        {
            string url = $"https://maps.googleapis.com/maps/api/place/details/json?place_id={placeId}&fields=rating,user_ratings_total&key={apiKey}";

            using HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            var data = JsonDocument.Parse(responseBody);

            if (data.RootElement.GetProperty("status").GetString() == "OK")
            {
                return new ExternalReviewModel()
                {
                    SiteName = "google.com",
                    SiteUrl = "https://g.page/r/CZ-5atXe9YTGEBM",
                    Score = (double)data.RootElement.GetProperty("result").GetProperty("rating").GetDecimal() * 20,
                    NumberOfReviews = data.RootElement.GetProperty("result").GetProperty("user_ratings_total").GetInt32(),
                    UpdateDate = DateOnly.FromDateTime(DateTime.Now)
                };
            };
            return null;
        }
    }
}
