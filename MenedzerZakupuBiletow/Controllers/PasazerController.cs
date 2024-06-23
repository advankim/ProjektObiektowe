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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pasażerowie.ToListAsync());
        }

        public async Task<IActionResult> Rezerwacje(int id)
        {
            var pasazer = await _context.Pasażerowie
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
    }
}
