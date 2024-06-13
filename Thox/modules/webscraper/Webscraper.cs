using HtmlAgilityPack;
using System.Diagnostics;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Thox.Models.DataModels;

namespace Thox.modules.webscraper
{
    public static class Webscraper
    {
        public static async Task<ExternalReviewModel> GetExternalReview(string url, string scoreXpath, string reviewCountXpath)
        {
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);

            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string pageContent = await response.Content.ReadAsStringAsync();
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(pageContent);

                var scoreNode = Regex.Replace(document.DocumentNode.SelectSingleNode(scoreXpath).InnerText.Trim(), @"[^\d.]", "");
                var reviewCountNode = Regex.Replace(document.DocumentNode.SelectSingleNode(reviewCountXpath).InnerText.Trim(), @"[^\d.]", "");

                if (scoreNode != null && reviewCountNode != null)
                {
                    // Convert the values to double
                    if (double.TryParse(scoreNode, out double score) &&
                        double.TryParse(reviewCountNode, out double numberOfReviews))
                    {
                        var uri = new Uri(url);
                        string siteName = uri.Host;
                        return new ExternalReviewModel
                        {
                            SiteName = siteName,
                            SiteUrl = url,
                            Score = score,
                            NumberOfReviews = numberOfReviews,
                            UpdateDate = DateOnly.FromDateTime(DateTime.Now)
                        };
                    }
                    else
                    {
                        Debug.WriteLine("Failed to parse score or number of reviews.");
                    }
                }
                else
                {
                    Debug.WriteLine("No article titles found.");
                }
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"Request error: {e.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Unexpected error: {e.Message}");
            }

            return null;
        }
    }
}
