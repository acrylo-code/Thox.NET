namespace Thox.Models.DataModels
{
    public class ExternalReviewModel
    {
        public int Id { get; set; }
        public string SiteName { get; set; }
        public string SiteUrl { get; set; }
        public double Score { get; set; }
        public double? NumberOfReviews { get; set; }
        public DateOnly UpdateDate { get; set; }
    }
}
