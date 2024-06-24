using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektObiektowe.Models;
using System.Threading.Tasks;

public class LotniskoDodajController : Controller
{
    private readonly BiletContext _context;

    public LotniskoDodajController(BiletContext context)
    {
        _context = context;
    }

    public IActionResult DodajLotnisko()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DodajLotnisko([Bind("Kod,Nazwa,Miasto,Kraj")] Lotnisko lotnisko)
    {
        if (ModelState.IsValid)
        {
            _context.Add(lotnisko);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "LotniskoIndex");
        }
        return View(lotnisko);
    }
}
