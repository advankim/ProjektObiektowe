using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ProjektObiektowe.Models;

namespace ProjektObiektowe.Controllers
{
    public class SamolotController : Controller
    {
        private readonly SamolotContext _context;

        public SamolotController(SamolotContext context)
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

      
        public IActionResult DodajSamolot()
        {
            return View(new Samolot());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajSamolot([Bind("Numer,Marka,Model,Pojemnosc_Klasa_1,Pojemnosc_Klasa_2,Linia")] Samolot samolot)
        {
            if (ModelState.IsValid)
            {
                // Sprawdzenie, czy samolot o podanym numerze już istnieje
                var existingSamolot = await _context.Samoloty.FirstOrDefaultAsync(s => s.Numer == samolot.Numer);

                if (existingSamolot != null)
                {
                    ModelState.AddModelError("Numer", "Samolot o podanym numerze już istnieje.");
                    return View(samolot);
                }

                // Ustawienie wartości Pojemnosc na podstawie Pojemnosc_Klasa_1 i Pojemnosc_Klasa_2
                samolot.Pojemnosc = samolot.Pojemnosc_Klasa_1 + samolot.Pojemnosc_Klasa_2;

                // Dodanie samolotu do bazy danych
                _context.Add(samolot);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Samolot");
            }

            return View(samolot);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EdytujSamolot(int id, [Bind("Id,Numer,Marka,Model,Pojemnosc,Pojemnosc_Linia_1,Pojemnosc_Linia_2, Linia")] Samolot samolot)
        {
            if (id != samolot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Aktualizacja samolotu w bazie danych
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
                return RedirectToAction(nameof(Index)); // Przekierowanie do listy samolotów po edycji
            }
            return View(samolot); // Zwróć widok z modelem samolotu, jeśli ModelState nie jest prawidłowy
        }

        private bool SamolotExists(int id)
        {
            return _context.Samoloty.Any(e => e.Id == id);
        }




    }
}
