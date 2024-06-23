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

            var pasazer = new Pasazer
            {
                Imie = imie,
                Nazwisko = nazwisko,
                Wiek = wiek,
                Plec = plec,
                PESEL = pesel
            };

            _context.Pasazerowie.Add(pasazer);
            await _context.SaveChangesAsync();

            // Pobierz cenę biletu na podstawie wybranej klasy
            decimal cenaBiletu = 0;
            if (klasa == "1")
            {
                cenaBiletu = await _context.Bilety.Where(b => b.Id == biletId).Select(b => b.Cena_Klasa_1).FirstOrDefaultAsync();
            }
            else if (klasa == "2")
            {
                cenaBiletu = await _context.Bilety.Where(b => b.Id == biletId).Select(b => b.Cena_Klasa_2).FirstOrDefaultAsync();
            }

            var rezerwacja = new Rezerwacja
            {
                Id_Pasazer = pasazer.Id,
                Id_Bilet = biletId,
                Data = DateTime.Now.ToString(),
                Status = "Nowa rezerwacja",
                Cena = cenaBiletu.ToString(), // Konwersja ceny na string
                Klasa = int.Parse(klasa),
                Bagaz = bagaz
            };

            _context.Rezerwacje.Add(rezerwacja);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Rezerwacja");
        }
    }
}