﻿using MenedzerZakupuBiletow.Models;
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

        public async Task<IActionResult> SprawdzPesel(string pesel)
        {
            var pasazer = await _context.Pasazerowie.FirstOrDefaultAsync(p => p.PESEL == pesel);
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
    }
}