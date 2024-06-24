using MenedzerZakupuBiletow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MenedzerZakupuBiletow.Controllers
{
    public class PasazerEditController : Controller
    {
        private readonly PasazerContext _context;
        public PasazerEditController(PasazerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasazer = await _context.Pasazerowie.FindAsync(id);

            if (pasazer == null)
            {
                return NotFound();
            }
            return View(pasazer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Imie,Nazwisko,Wiek,Plec,PESEL")] Pasazer pasazer)
        {
            if (id != pasazer.Id)
            {
                return NotFound();
            }

            Console.WriteLine(ModelState.IsValid);

            foreach (var modelStateKey in ModelState.Keys)
            {
                var modelStateVal = ModelState[modelStateKey];
                foreach (var error in modelStateVal.Errors)
                {
                    Console.WriteLine($"Błąd walidacji dla {modelStateKey}: {error.ErrorMessage}");
                }
            }

            ModelState.Remove("Rezerwacje");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pasazer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PasazerExists(pasazer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Pasazer");
            }

            return View(pasazer);
        }
        private bool PasazerExists(int id)
        {
            return _context.Pasazerowie.Any(e => e.Id == id);
        }
    }
}
