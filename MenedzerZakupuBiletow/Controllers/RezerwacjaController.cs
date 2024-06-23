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
    }
}