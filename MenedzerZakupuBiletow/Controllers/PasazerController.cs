using MenedzerZakupuBiletow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MenedzerZakupuBiletow.Controllers
{
    public class PasazerController : Controller
    {
        private readonly PasazerContext _context;

        public PasazerController(PasazerContext context)
        {
            _context = context;
        }
       /* public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Imie,Nazwisko,Wiek,Plec,PESEL,Bagaz")] Pasazer pasazer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pasazer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pasazer);
        }*/

        public async Task<IActionResult> Index()
        {
            return View(await _context.Pasazerowie.ToListAsync());
        }
    }
}
