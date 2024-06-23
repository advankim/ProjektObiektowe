using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektObiektowe.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ProjektObiektowe.Controllers
{
    public class BiletController : Controller
    {
        private readonly BiletContext _context;
        private readonly ILogger<BiletController> _logger;

        public BiletController(BiletContext context, ILogger<BiletController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult DodajBilet()
        {
            ViewBag.Loty = new SelectList(_context.Loty.Include(l => l.Samolot), "Id", "Numer");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajBilet([Bind("Id_Lot,Numer,Cena_Klasa_1,Cena_Klasa_2")] Bilet bilet)
        {
            var lot = await _context.Loty
                .Include(l => l.Samolot)
                .FirstOrDefaultAsync(l => l.Id == bilet.Id_Lot);

            if (lot == null)
            {
                ModelState.AddModelError("Id_Lot", "Wybrany lot nie istnieje.");
                ViewBag.Loty = new SelectList(_context.Loty.Include(l => l.Samolot), "Id", "Numer", bilet.Id_Lot);
                return View(bilet);
            }

            bilet.Dostepnych_Klasa_1 = lot.Samolot.Pojemnosc_Klasa_1;
            bilet.Dostepnych_Klasa_2 = lot.Samolot.Pojemnosc_Klasa_2;

            // Sprawdź unikalność Id_Lot i Numer
            var existingBilet = await _context.Bilety
                .FirstOrDefaultAsync(b => b.Id_Lot == bilet.Id_Lot && b.Numer == bilet.Numer);

            if (existingBilet != null)
            {
                ModelState.AddModelError("Numer", "Bilet o podanym numerze dla tego lotu już istnieje.");
                ViewBag.Loty = new SelectList(_context.Loty.Include(l => l.Samolot), "Id", "Numer", bilet.Id_Lot);
                return View(bilet);
            }

            _context.Add(bilet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> UsunBilet(int? id)
        {
            var bilet = await _context.Bilety.FindAsync(id);
            if (bilet == null)
            {
                return NotFound();
            }

            return View(bilet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsunBilet(int id)
        {
            var bilet = await _context.Bilety.FindAsync(id);
            if (bilet == null)
            {
                return NotFound();
            }

            _context.Bilety.Remove(bilet);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Index()
        {
            var bilety = await _context.Bilety.Include(b => b.Lot).ThenInclude(l => l.Samolot).ToListAsync();
            return View(bilety);
        }
    }
}
