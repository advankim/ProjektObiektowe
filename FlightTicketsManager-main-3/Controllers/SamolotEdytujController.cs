using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektObiektowe.Models;

namespace ProjektObiektowe.Controllers
{
    public class SamolotEdytujController : Controller
    {
        private readonly SamolotContext _context;

        public SamolotEdytujController(SamolotContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> EdytujSamolot(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var samolot = await _context.Samoloty.FindAsync(id);
            if (samolot == null)
            {
                return NotFound();
            }

            var associatedFlights = await _context.Loty.AnyAsync(l => l.Id_Samolot == id);
            if (associatedFlights)
            {
                return RedirectToAction("BladEdycji");
            }

            return View(samolot);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EdytujSamolot(int id, [Bind("Id,Numer,Marka,Model,Pojemnosc_Klasa_1,Pojemnosc_Klasa_2,Linia")] Samolot samolot)
        {
            if (id != samolot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    samolot.Pojemnosc = samolot.Pojemnosc_Klasa_1 + samolot.Pojemnosc_Klasa_2;
                    _context.Update(samolot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SamolotExists(samolot.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "SamolotIndex");
            }
            return View(samolot);
        }

        private bool SamolotExists(int id)
        {
            return _context.Samoloty.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult BladEdycji()
        {
            return View();
        }
    }
}
