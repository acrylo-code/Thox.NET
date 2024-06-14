using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Thox.Data;
using Thox.Models.DataModels;
using Thox.Models.ViewModels;
using Thox.Models.ViewModels.Home;
using Thox.Services;

namespace Thox.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly GetReviewsFromExternalSites _getReviewsFromExternalSites;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, GetReviewsFromExternalSites getReviewsFromExternalSites)
        {
            _logger = logger;
            _context = context;
            _getReviewsFromExternalSites = getReviewsFromExternalSites;
        }

        public async Task<IActionResult> Index()
        {
            //await _getReviewsFromExternalSites.GetReviews();
            var model = new HomeViewModel
            {
                RoomPrices = _context.Prices.ToList(),
                ExternalReviews = _context.ExternalReviews.ToList()
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation("User accessed the privacy page");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}