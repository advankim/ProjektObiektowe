using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenedzerZakupuBiletow.Models;

namespace MenedzerZakupuBiletow.Controllers
{
    public class BiletController : Controller
    {
        private readonly RezerwacjaContext _context;

        public BiletController(RezerwacjaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bilety = await _context.Bilety.ToListAsync();
            return View(bilety);
        }

        public async Task<IActionResult> WybierzBilet(int id)
        {
            var bilet = await _context.Bilety.FindAsync(id);
            if (bilet == null)
            {
                return NotFound();
            }

            ViewData["NumerBiletu"] = bilet.Numer;
            ViewData["BiletId"] = bilet.Id;

            return View();
        }

        [HttpPost]
        public IActionResult WybierzBilet(int biletId, string klasa, string bagaz)
        {
            TempData["BiletId"] = biletId;
            TempData["Klasa"] = klasa;
            TempData["Bagaz"] = bagaz;

            return RedirectToAction("WprowadzDanePasazera");
        }

        public IActionResult WprowadzDanePasazera()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WprowadzDanePasazera(string imie, string nazwisko, int wiek, string plec, string pesel)
        {
            var biletId = (int)TempData["BiletId"];
            var klasa = (string)TempData["Klasa"];
            var bagaz = (string)TempData["Bagaz"];

            var pasazer = await _context.Pasazerowie.FirstOrDefaultAsync(p => p.PESEL == pesel);

            if (pasazer == null)
            {
                // Dodanie nowego pasażera
                pasazer = new Pasazer
                {
                    Imie = imie,
                    Nazwisko = nazwisko,
                    Wiek = wiek,
                    Plec = plec,
                    PESEL = pesel
                };
                _context.Pasazerowie.Add(pasazer);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Sprawdzenie zgodności danych
                if (pasazer.Imie != imie || pasazer.Nazwisko != nazwisko || pasazer.Wiek != wiek || pasazer.Plec != plec)
                {
                    ModelState.AddModelError("", "Dane pasażera nie zgadzają się z danymi w bazie.");
                    return View();
                }
            }

            // Pobierz cenę biletu na podstawie wybranej klasy
            var bilet = await _context.Bilety.FindAsync(biletId);
            if (bilet == null)
            {
                return NotFound();
            }

            decimal cenaBiletu = 0;
            if (klasa == "1")
            {
                cenaBiletu = bilet.Cena_Klasa_1;
                bilet.Dostepnych_Klasa_1 -= 1;
            }
            else if (klasa == "2")
            {
                cenaBiletu = bilet.Cena_Klasa_2;
                bilet.Dostepnych_Klasa_2 -= 1;
            }

            var rezerwacja = new Rezerwacja
            {
                Id_Pasazer = pasazer.Id,
                Id_Bilet = biletId,
                Data = DateTime.Now.ToString(),
                Status = "Nowa rezerwacja",
                Cena = cenaBiletu.ToString(),
                Klasa = int.Parse(klasa),
                Bagaz = bagaz
            };

            _context.Rezerwacje.Add(rezerwacja);
            await _context.SaveChangesAsync();

            // Zapisz zmiany w ilości dostępnych miejsc
            _context.Bilety.Update(bilet);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Rezerwacja");
        }


        public async Task<IActionResult> GetAvailability(int biletId, int klasa)
        {
            var bilet = await _context.Bilety.FindAsync(biletId);
            if (bilet == null)
            {
                return NotFound();
            }

            int dostepnosc = 0;
            if (klasa == 1)
            {
                dostepnosc = bilet.Dostepnych_Klasa_1;
            }
            else if (klasa == 2)
            {
                dostepnosc = bilet.Dostepnych_Klasa_2;
            }

            return Json(new { dostepnosc });
        }

        public async Task<IActionResult> SprawdzDostepnosc(int biletId, int klasa)
        {
            var bilet = await _context.Bilety.FindAsync(biletId);
            if (bilet == null)
            {
                return NotFound();
            }

            int dostepnosc = 0;
            if (klasa == 1)
            {
                dostepnosc = bilet.Dostepnych_Klasa_1;
            }
            else if (klasa == 2)
            {
                dostepnosc = bilet.Dostepnych_Klasa_2;
            }

            return Json(new { dostepnosc });
        }
    }
}