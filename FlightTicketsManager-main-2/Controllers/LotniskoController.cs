using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektObiektowe.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class LotniskoController : Controller
{
    private readonly LotniskoContext _context;

    public LotniskoController(LotniskoContext context)
    {
        _context = context;
    }


    // GET: Lotnisko/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Lotnisko/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Kod,Nazwa,Miasto,Kraj")] Lotnisko lotnisko)
    {
        if (ModelState.IsValid)
        {
            _context.Add(lotnisko);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(lotnisko);
    }

    // GET: Lotnisko
    public async Task<IActionResult> Index()
    {
        return View(await _context.Lotniska.ToListAsync());
    }
}
