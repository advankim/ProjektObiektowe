using MenedzerZakupuBiletow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MenedzerZakupuBiletow.Controllers
{
    public class PasazerController : Controller
    {
        private readonly RezerwacjaContext _context;

        public PasazerController(RezerwacjaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Pasazerowie.ToListAsync());
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasazer = await _context.Pasazerowie
                .Include(p => p.Rezerwacje)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pasazer == null)
            {
                return NotFound();
            }

            if (pasazer.Rezerwacje != null && pasazer.Rezerwacje.Any())
            {
                ViewBag.ErrorMessage = "Nie można usunąć pasażera, który ma powiązane rezerwacje.";
            }

            return View(pasazer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pasazer = await _context.Pasazerowie
                .Include(p => p.Rezerwacje)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pasazer == null)
            {
                return NotFound();
            }

            if (pasazer.Rezerwacje == null || !pasazer.Rezerwacje.Any())
            {
                _context.Pasazerowie.Remove(pasazer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.ErrorMessage = "Nie można usunąć pasażera, który ma powiązane rezerwacje.";
                return View(pasazer);
            }
        }
        public async Task<IActionResult> Rezerwacje(int id)
        {
            var pasazer = await _context.Pasazerowie
                .Include(p => p.Rezerwacje)
                .ThenInclude(r => r.Bilet)
                .ThenInclude(b => b.Lot)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pasazer == null)
            {
                return NotFound();
            }

            return View(pasazer);
        }


        public IActionResult WprowadzDanePasazera()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WprowadzDanePasazera(Pasazer pasazer)
        {
            if (ModelState.IsValid)
            {
                var existingPasazer = await _context.Pasazerowie
                    .FirstOrDefaultAsync(p => p.PESEL == pasazer.PESEL);

                if (existingPasazer != null)
                {
                    ModelState.AddModelError(string.Empty, "Pasażer o tym PESEL już istnieje.");
                }
                else
                {
                    _context.Add(pasazer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(pasazer);
        }


        public async Task<IActionResult> SprawdzPesel(string pesel)
        {
            var pasazer = await _context.Pasazerowie
                .FirstOrDefaultAsync(p => p.PESEL == pesel);

            if (pasazer == null)
            {
                return Json(new { exists = false });
            }

            return Json(new
            {
                exists = true,
                imie = pasazer.Imie,
                nazwisko = pasazer.Nazwisko,
                wiek = pasazer.Wiek,
                plec = pasazer.Plec
            });
        }

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

            ModelState.Remove("Rezerwacje");

            if (_context.Pasazerowie.Any(p => p.PESEL == pasazer.PESEL && p.Id != pasazer.Id))
            {
                ModelState.AddModelError("PESEL", "Pasażer z tym numerem PESEL już istnieje.");
            }

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
                return RedirectToAction(nameof(Index));
            }
            return View(pasazer);
        }

        private bool PasazerExists(int id)
        {
            return _context.Pasazerowie.Any(e => e.Id == id);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Imie,Nazwisko,Wiek,Plec,PESEL")] Pasazer pasazer)
        {
            ModelState.Remove("Rezerwacje");

            if (_context.Pasazerowie.Any(p => p.PESEL == pasazer.PESEL))
            {
                ModelState.AddModelError("PESEL", "Pasażer z tym numerem PESEL już istnieje.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(pasazer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pasazer);
        }
    }
}