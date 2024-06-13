using Thox.Models.DataModels;

namespace Thox.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<RoomPrice> RoomPrices { get; set; }
        public List<ExternalReviewModel> ExternalReviews { get; set; }
    }
}
