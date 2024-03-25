using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Thox.Models;

namespace Thox.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Log information
            _logger.LogInformation("User accessed the home page");
            return View();
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