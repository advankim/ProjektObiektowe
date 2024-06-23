using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektObiektowe.Models;

namespace ProjektObiektowe.Controllers
{
    public class SamolotUsunController : Controller
    {
        private readonly SamolotContext _context;

        public SamolotUsunController(SamolotContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> UsunSamolot(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var samolot = await _context.Samoloty.FirstOrDefaultAsync(m => m.Id == id);
            if (samolot == null)
            {
                return NotFound();
            }

            return View(samolot);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsunSamolot(int id)
        {
            var samolot = await _context.Samoloty.FindAsync(id);

            var associatedFlights = await _context.Loty.AnyAsync(l => l.Id_Samolot == id);
            if (associatedFlights)
            {
                return RedirectToAction("BladUsuwania");
            }

            _context.Samoloty.Remove(samolot);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "SamolotIndex");
        }

        [HttpGet]
        public IActionResult BladUsuwania()
        {
            return View();
        }
    }
}
