using MenedzerZakupuBiletow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var bilety = await _context.Bilety.Include(b => b.Lot).ToListAsync();
            return View(bilety);
        }

        [HttpPost]
        public IActionResult WybierzBilet(int id)
        {
            var bilet = _context.Bilety.Include(b => b.Lot).FirstOrDefault(b => b.Id == id);
            if (bilet == null)
            {
                return NotFound();
            }

            var model = new WybierzBiletViewModel
            {
                Bilet = bilet,
                Klasa = 1, // Domyślnie klasa 1
                Bagaz = string.Empty
            };

            return View("FormularzBilet", model);
        }

        [HttpPost]
        public IActionResult WypelnijDanePasazera(WybierzBiletViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bilet = _context.Bilety.Include(b => b.Lot).FirstOrDefault(b => b.Id == model.Bilet.Id);
                if (bilet == null)
                {
                    return NotFound();
                }

                var rezerwacjaModel = new RezerwacjaViewModel
                {
                    Bilet = bilet,
                    Klasa = model.Klasa,
                    Bagaz = model.Bagaz,
                    Pasazer = new Pasazer()
                };

                return View("FormularzPasazer", rezerwacjaModel);
            }

            return View("FormularzBilet", model);
        }

        [HttpPost]
        public IActionResult Zarezerwuj(RezerwacjaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pasazer = new Pasazer
                {
                    Imie = model.Pasazer.Imie,
                    Nazwisko = model.Pasazer.Nazwisko,
                    Wiek = model.Pasazer.Wiek,
                    Plec = model.Pasazer.Plec,
                    PESEL = model.Pasazer.PESEL
                };

                _context.Pasazerowie.Add(pasazer);
                _context.SaveChanges();

                var rezerwacja = new Rezerwacja
                {
                    Id_Pasazer = pasazer.Id,
                    Id_Bilet = model.Bilet.Id,
                    Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Status = "Zarezerwowane",
                    Cena = (model.Klasa == 1 ? model.Bilet.Cena_Klasa_1 : model.Bilet.Cena_Klasa_2).ToString(),
                    Klasa = model.Klasa,
                    Bagaz = model.Bagaz
                };

                _context.Rezerwacje.Add(rezerwacja);
                _context.SaveChanges();

                return RedirectToAction("Szczegoly", "Rezerwacja", new { id = rezerwacja.Id });
            }

            return View("FormularzPasazer", model);
        }
    }
}