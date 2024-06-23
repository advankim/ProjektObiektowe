using Microsoft.AspNetCore.Mvc;
using ProjektObiektowe.Models;
using System.Diagnostics;

namespace ProjektObiektowe.Controllers
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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult GoToSamolotyIndex()
        {
            return RedirectToAction("Index", "Samolot");
        }

        public IActionResult GoToLotyIndex()
        {
            return RedirectToAction("Index", "Lot");
        }

        public IActionResult GoToLotniskaIndex()
        {
            return RedirectToAction("Index", "Lotnisko");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
