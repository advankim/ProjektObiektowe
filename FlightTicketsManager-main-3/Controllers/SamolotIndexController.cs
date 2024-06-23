using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektObiektowe.Models;

namespace ProjektObiektowe.Controllers
{
    public class SamolotIndexController : Controller
    {
        private readonly SamolotContext _context;

        public SamolotIndexController(SamolotContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Samoloty.ToListAsync());
        }

        public async Task<IActionResult> WybierzSamolot()
        {
            var samoloty = await _context.Samoloty.ToListAsync();
            return View(samoloty);
        }
    }
}
