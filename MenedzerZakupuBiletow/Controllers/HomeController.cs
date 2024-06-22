using Microsoft.AspNetCore.Mvc;

namespace MenedzerZakupuBiletow.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}