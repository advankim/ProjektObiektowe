using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjektObiektowe.Models;
using Microsoft.EntityFrameworkCore;

public class LotController : Controller
{
    private readonly LotContext _context;

    public LotController(LotContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var loty = await _context.Loty
            .Include(l => l.Samolot)
            .Include(l => l.Lotnisko_Wylot)
            .Include(l => l.Lotnisko_Przylot)
            .ToListAsync();
        return View(loty);
    }
    

    [HttpGet]
    public async Task<IActionResult> WybierzKraje()
    {
        var kraje = await _context.Lotniska
            .Select(l => l.Kraj)
            .Distinct()
            .ToListAsync();

        ViewBag.Kraje = kraje;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult WybierzKraje(string selectedKrajWylotu, string selectedKrajPrzylotu)
    {
        if (string.IsNullOrEmpty(selectedKrajWylotu) || string.IsNullOrEmpty(selectedKrajPrzylotu))
        {
            return RedirectToAction(nameof(WybierzKraje));
        }

        return RedirectToAction("WybierzLotniska", new { selectedKrajWylotu, selectedKrajPrzylotu });
    }

    [HttpGet]
    public async Task<IActionResult> EdytujKraje(int id)
    {
        var lot = await _context.Loty
            .Include(l => l.Lotnisko_Wylot)
            .Include(l => l.Lotnisko_Przylot)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (lot == null)
        {
            return NotFound();
        }

        var kraje = await _context.Lotniska
            .Select(l => l.Kraj)
            .Distinct()
            .ToListAsync();

        ViewBag.LotId = id;
        ViewBag.SelectedKrajWylotu = new SelectList(kraje, lot.Lotnisko_Wylot.Kraj);
        ViewBag.SelectedKrajPrzylotu = new SelectList(kraje, lot.Lotnisko_Przylot.Kraj);

        return View(lot);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EdytujKraje(int id, string selectedKrajWylotu, string selectedKrajPrzylotu)
    {
        if (string.IsNullOrEmpty(selectedKrajWylotu) || string.IsNullOrEmpty(selectedKrajPrzylotu))
        {
            return RedirectToAction(nameof(WybierzKraje), new { id });
        }

        return RedirectToAction("EdytujLot", new { id, selectedKrajWylotu, selectedKrajPrzylotu });
    }


    [HttpGet]
    public async Task<IActionResult> DodajLot(string selectedKrajWylotu = null, string selectedKrajPrzylotu = null)
    {
        var kraje = await _context.Lotniska
            .Select(l => l.Kraj)
            .Distinct()
            .ToListAsync();

       // var samoloty = await _context.Samoloty.ToListAsync();
        var samoloty = await _context.Samoloty
        .Select(s => new
        {
            s.Id,
            Description = $"{s.Numer} - {s.Marka} {s.Model} {s.Linia} Liczba miejsc: {s.Pojemnosc}"
        })
        .ToListAsync();

        ViewBag.Kraje = kraje;
        ViewBag.SelectedKrajWylotu = selectedKrajWylotu;
        ViewBag.SelectedKrajPrzylotu = selectedKrajPrzylotu;
        ViewBag.Samoloty = new SelectList(samoloty, "Id", "Description");

        if (!string.IsNullOrEmpty(selectedKrajWylotu))
        {
            var lotniskaWylotu = await _context.Lotniska
                .Where(l => l.Kraj == selectedKrajWylotu)
                .ToListAsync();
            ViewBag.LotniskaWylotu = new SelectList(lotniskaWylotu, "Id", "Nazwa");
        }
        else
        {
            ViewBag.LotniskaWylotu = new SelectList(Enumerable.Empty<SelectListItem>());
        }

        if (!string.IsNullOrEmpty(selectedKrajPrzylotu))
        {
            var lotniskaPrzylotu = await _context.Lotniska
                .Where(l => l.Kraj == selectedKrajPrzylotu)
                .ToListAsync();
            ViewBag.LotniskaPrzylotu = new SelectList(lotniskaPrzylotu, "Id", "Nazwa");
        }
        else
        {
            ViewBag.LotniskaPrzylotu = new SelectList(Enumerable.Empty<SelectListItem>());
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DodajLot(int selectedLotniskoWylotuId, int selectedLotniskoPrzylotuId, string numerLotu, DateTime czasWylot, DateTime czasPrzylot, int idSamolotu)
    {

        var existingLot = await _context.Loty.FirstOrDefaultAsync(l => l.Numer == numerLotu);
        if (existingLot != null)
        {
            ModelState.AddModelError("numerLotu", "Lot o podanym numerze już istnieje.");
        }
        if (ModelState.IsValid)
        {
            var lot = new Lot
            {
                Numer = numerLotu,
                Czas_Wylot = czasWylot,
                Czas_Przylot = czasPrzylot,
                Id_Lotnisko_Wylot = selectedLotniskoWylotuId,
                Id_Lotnisko_Przylot = selectedLotniskoPrzylotuId,
                Id_Samolot = idSamolotu
            };

            _context.Add(lot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Potwierdzenie), new { id = lot.Id });
            
        }

        var lotniska = await _context.Lotniska.ToListAsync();
        var samoloty = await _context.Samoloty.ToListAsync();

        ViewBag.LotniskaWylotu = new SelectList(lotniska, "Id", "Nazwa");
        ViewBag.LotniskaPrzylotu = new SelectList(lotniska, "Id", "Nazwa");
        ViewBag.Samoloty = new SelectList(samoloty, "Id", "Numer");

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Potwierdzenie(int id)
    {
        var lot = await _context.Loty
            .Include(l => l.Lotnisko_Wylot)
            .Include(l => l.Lotnisko_Przylot)
            .Include(l => l.Samolot)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (lot == null)
        {
            return NotFound();
        }

        return View(lot);
    }


    // GET: Loty/Details/5
    public async Task<IActionResult> Szczegoly(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lot = await _context.Loty
            .Include(l => l.Samolot)
            .Include(l => l.Lotnisko_Wylot)
            .Include(l => l.Lotnisko_Przylot)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (lot == null)
        {
            return NotFound();
        }

        return View(lot);
    }

    [HttpGet]
    public async Task<IActionResult> EdytujLot(int id, string selectedKrajWylotu, string selectedKrajPrzylotu)
    {
        var lot = await _context.Loty
            .Include(l => l.Lotnisko_Wylot)
            .Include(l => l.Lotnisko_Przylot)
            .Include(l => l.Samolot)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (lot == null)
        {
            return NotFound();
        }

        var lotniskaWylotu = await _context.Lotniska
            .Where(l => l.Kraj == selectedKrajWylotu)
            .ToListAsync();

        var lotniskaPrzylotu = await _context.Lotniska
            .Where(l => l.Kraj == selectedKrajPrzylotu)
            .ToListAsync();

        var samoloty = await _context.Samoloty
            .Select(s => new
            {
                s.Id,
                Description = $"{s.Numer} - {s.Marka} {s.Model} {s.Linia} Liczba miejsc: {s.Pojemnosc}"
            })
            .ToListAsync();

        ViewBag.LotniskaWylotu = new SelectList(lotniskaWylotu, "Id", "Nazwa", lot.Id_Lotnisko_Wylot);
        ViewBag.LotniskaPrzylotu = new SelectList(lotniskaPrzylotu, "Id", "Nazwa", lot.Id_Lotnisko_Przylot);
        ViewBag.Samoloty = new SelectList(samoloty, "Id", "Description", lot.Id_Samolot); // Ustawienie domyślnego samolotu

        return View(lot);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EdytujLot(int id, int selectedLotniskoWylotuId, int selectedLotniskoPrzylotuId, string numerLotu, DateTime czasWylot, DateTime czasPrzylot, int idSamolotu)
    {
        // Sprawdenie, czy numer lotu już istnieje (z wyjątkiem bieżącego lotu)
        var existingLot = await _context.Loty
            .Where(l => l.Numer == numerLotu && l.Id != id)
            .FirstOrDefaultAsync();
        if (existingLot != null)
        {
            ModelState.AddModelError("numerLotu", "Lot o podanym numerze już istnieje.");
        }

        if (ModelState.IsValid)
        {
            //Lot z bazy danych
            var lot = await _context.Loty.FindAsync(id);
            if (lot == null)
            {
                return NotFound();
            }

            
            lot.Numer = numerLotu;
            lot.Czas_Wylot = czasWylot;
            lot.Czas_Przylot = czasPrzylot;
            lot.Id_Lotnisko_Wylot = selectedLotniskoWylotuId;
            lot.Id_Lotnisko_Przylot = selectedLotniskoPrzylotuId;
            lot.Id_Samolot = idSamolotu;

            //Update
            _context.Update(lot);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Potwierdzenie), new { id = lot.Id });
        }

        var lotniska = await _context.Lotniska.ToListAsync();
        var samoloty = await _context.Samoloty
            .Select(s => new
            {
                s.Id,
                Description = $"{s.Numer} - {s.Marka} {s.Model} {s.Linia} Liczba miejsc: {s.Pojemnosc}"
            })
            .ToListAsync();

        ViewBag.LotniskaWylotu = new SelectList(lotniska, "Id", "Nazwa", selectedLotniskoWylotuId);
        ViewBag.LotniskaPrzylotu = new SelectList(lotniska, "Id", "Nazwa", selectedLotniskoPrzylotuId);
        ViewBag.Samoloty = new SelectList(samoloty, "Id", "Description", idSamolotu); 

        
        return View(await _context.Loty.FindAsync(id));
    }


   
    [HttpGet]
    public async Task<IActionResult> UsunLot(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lot = await _context.Loty
            .Include(l => l.Lotnisko_Wylot)
            .Include(l => l.Lotnisko_Przylot)
            .Include(l => l.Samolot)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (lot == null)
        {
            return NotFound();
        }

        return View(lot);
    }

    [HttpPost]
    public async Task<IActionResult> UsunLot(int id)
    {
        var lot = await _context.Loty
                                .Include(l => l.Bilety)
                                .FirstOrDefaultAsync(l => l.Id == id);

        if (lot == null)
        {
            return NotFound();
        }

        if (lot.Bilety.Any())
        {
            ModelState.AddModelError("", "Nie można usunąć lotu, ponieważ istnieją przypisane do niego bilety.");
            return RedirectToAction("BladUsuwania");
        }

        _context.Loty.Remove(lot);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

  
    [HttpGet]
    public IActionResult BladUsuwania()
    {
        return View();
    }






    private async Task PopulateViewBagsAsync(string selectedKrajWylotu = null, string selectedKrajPrzylotu = null)
    {
        var kraje = await _context.Lotniska.Select(l => l.Kraj).Distinct().ToListAsync();
        ViewBag.Kraje = new SelectList(kraje);

        if (!string.IsNullOrEmpty(selectedKrajWylotu))
        {
            var lotniskaWylotu = await _context.Lotniska.Where(l => l.Kraj == selectedKrajWylotu).ToListAsync();
            ViewBag.LotniskaWylotu = new SelectList(lotniskaWylotu, "Id", "Nazwa");
        }
        else
        {
            ViewBag.LotniskaWylotu = new SelectList(new List<Lotnisko>(), "Id", "Nazwa");
        }

        if (!string.IsNullOrEmpty(selectedKrajPrzylotu))
        {
            var lotniskaPrzylotu = await _context.Lotniska.Where(l => l.Kraj == selectedKrajPrzylotu).ToListAsync();
            ViewBag.LotniskaPrzylotu = new SelectList(lotniskaPrzylotu, "Id", "Nazwa");
        }
        else
        {
            ViewBag.LotniskaPrzylotu = new SelectList(new List<Lotnisko>(), "Id", "Nazwa");
        }
    }




}
