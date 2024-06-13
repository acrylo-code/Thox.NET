using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Thox.Data;
using Thox.Models.DataModels;
using Thox.modules.ExternalAPI;
using Thox.modules.webscraper;

namespace Thox.Services
{
    public class GetReviewsFromExternalSites
    {
        private readonly ApplicationDbContext _context;

        public GetReviewsFromExternalSites(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task GetReviews()
        {
            try
            {
                var externalReviewTasks = new List<Task<ExternalReviewModel>>
            {
                // Get score for escapetalk.nl
                Webscraper.GetExternalReview("https://escapetalk.nl/escaperoom/escaperoom-coevorden/", "//div[@class='score']/p", "//p[@class='based-on']"),
                // Get score from escaperoomsnederland.nl
                Webscraper.GetExternalReview("https://escaperoomsnederland.nl/rooms/escaperoom-coevorden/", "//span[contains(@class, 'ern-rating-value') and @itemprop='ratingValue']", "//strong[@itemprop='reviewCount']"),
                // Get score from Google
                GoogleReview.GetReviewsGoogle(Settings.GetApiKey("GoogleReview_ApiKey"), Settings.GetApiKey("GoogleReview_PlaceId"))
            };

                var externalReviewResults = await Task.WhenAll(externalReviewTasks);

                List<ExternalReviewModel> externalReviewScore = externalReviewResults.ToList();

                // Find the same external review in the database and update it
                foreach (var item in externalReviewScore)
                {
                    Debug.WriteLine($"Site: {item.SiteName}, Url: {item.SiteUrl}, Review Score: {item.Score}, Item Count: {item.NumberOfReviews}");

                    var review = await _context.ExternalReviews.FirstOrDefaultAsync(x => x.SiteName == item.SiteName);
                    if (review != null)
                    {
                        review.Score = item.Score;
                        review.SiteUrl = item.SiteUrl;
                        review.NumberOfReviews = item.NumberOfReviews;
                        review.UpdateDate = item.UpdateDate;
                    }
                    else
                    {
                        _context.ExternalReviews.Add(item);
                    }
                }

                await _context.SaveChangesAsync(); // Ensure you save the changes to the database
            }
            catch (Exception ex)
            {
                // Handle exceptions accordingly
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}

