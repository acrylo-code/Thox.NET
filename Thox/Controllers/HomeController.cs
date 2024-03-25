using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using System.Diagnostics;
using Thox.Data;
using Thox.Models;

namespace Thox.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Log information
            _logger.LogInformation("User accessed the home page");
			//create a model with all roomprices
            List<RoomPrice> roomPrices = new List<RoomPrice>();
			foreach (var room in _context.Prices)
            {
                roomPrices.Add(room);
			}
			return View(roomPrices);
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