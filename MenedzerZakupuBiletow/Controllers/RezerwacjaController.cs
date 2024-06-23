using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenedzerZakupuBiletow.Models;

namespace MenedzerZakupuBiletow.Controllers
{
    public class RezerwacjaController : Controller
    {
        private readonly RezerwacjaContext _context;

        public RezerwacjaController(RezerwacjaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Szczegoly(int id)
        {
            var rezerwacja = await _context.Rezerwacje
                .Include(r => r.Pasazer)
                .Include(r => r.Bilet)
                    .ThenInclude(b => b.Lot)
                        .ThenInclude(l => l.LotniskoWylot)
                .Include(r => r.Bilet)
                    .ThenInclude(b => b.Lot)
                        .ThenInclude(l => l.LotniskoPrzylot)
                .Include(r => r.Bilet)
                    .ThenInclude(b => b.Lot)
                        .ThenInclude(l => l.Samolot)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rezerwacja == null)
            {
                return NotFound();
            }

            return View(rezerwacja);
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Rezerwacje.ToListAsync());
        }

        public async Task<IActionResult> Anuluj(int id)
        {
            var rezerwacja = await _context.Rezerwacje.FindAsync(id);
            if (rezerwacja == null)
            {
                return NotFound();
            }

            if (rezerwacja.Status != "Anulowana")
            {
                // Zmiana statusu na "Anulowana"
                rezerwacja.Status = "Anulowana";

                // Przywrócenie miejsca
                var bilet = await _context.Bilety.FindAsync(rezerwacja.Id_Bilet);
                if (bilet != null)
                {
                    if (rezerwacja.Klasa == 1)
                    {
                        bilet.Dostepnych_Klasa_1 += 1;
                    }
                    else if (rezerwacja.Klasa == 2)
                    {
                        bilet.Dostepnych_Klasa_2 += 1;
                    }
                }

                _context.Rezerwacje.Update(rezerwacja);
                _context.Bilety.Update(bilet);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Usun(int id)
        {
            var rezerwacja = await _context.Rezerwacje.FindAsync(id);
            if (rezerwacja == null)
            {
                return NotFound();
            }

            if (rezerwacja.Status != "Anulowana")
            {
                // Przywrócenie miejsca
                var bilet = await _context.Bilety.FindAsync(rezerwacja.Id_Bilet);
                if (bilet != null)
                {
                    if (rezerwacja.Klasa == 1)
                    {
                        bilet.Dostepnych_Klasa_1 += 1;
                    }
                    else if (rezerwacja.Klasa == 2)
                    {
                        bilet.Dostepnych_Klasa_2 += 1;
                    }
                }

                _context.Bilety.Update(bilet);
            }

            _context.Rezerwacje.Remove(rezerwacja);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}