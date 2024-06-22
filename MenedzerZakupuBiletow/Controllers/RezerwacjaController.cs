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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Id_Pasazer,Id_Bilet")] Rezerwacja rezerwacja)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rezerwacja);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rezerwacja);
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Rezerwacje.ToListAsync());
        }
    }
}