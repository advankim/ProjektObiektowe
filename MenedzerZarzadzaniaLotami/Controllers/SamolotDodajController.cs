﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektObiektowe.Models;

namespace ProjektObiektowe.Controllers
{
    public class SamolotDodajController : Controller
    {
        private readonly SamolotContext _context;

        public SamolotDodajController(SamolotContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult DodajSamolot()
        {
            return View(new Samolot());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajSamolot([Bind("Numer,Marka,Model,Rzedy_Klasa_1,Rzedy_Klasa_2,Linia")] Samolot samolot)
        {
            if (ModelState.IsValid)
            {
                var existingSamolot = await _context.Samoloty.FirstOrDefaultAsync(s => s.Numer == samolot.Numer);

                if (existingSamolot != null)
                {
                    ModelState.AddModelError("Numer", "Samolot o podanym numerze już istnieje.");
                    return View(samolot);
                }

                samolot.Pojemnosc = samolot.Rzedy_Klasa_1*4 + samolot.Rzedy_Klasa_2*6;
                _context.Add(samolot);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "SamolotIndex");
            }

            return View(samolot);
        }
    }
}
