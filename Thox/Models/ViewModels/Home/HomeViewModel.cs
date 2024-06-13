using Thox.Models.DataModels;
using Thox.Models.DataModels.Review;

namespace Thox.Models.ViewModels.Home
{
    public class HomeViewModel
    {
        public List<RoomPrice> RoomPrices { get; set; }
        public List<ExternalReviewModel> ExternalReviews { get; set; }
    }
}
