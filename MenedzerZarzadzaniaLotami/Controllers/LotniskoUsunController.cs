using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektObiektowe.Models;
using System.Threading.Tasks;

public class LotniskoUsunController : Controller
{
    private readonly BiletContext _context;

    public LotniskoUsunController(BiletContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> UsunLotnisko(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lotnisko = await _context.Lotniska
            .FirstOrDefaultAsync(m => m.Id == id);
        if (lotnisko == null)
        {
            return NotFound();
        }

        return View(lotnisko);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UsunLotnisko(int id)
    {
        var lotnisko = await _context.Lotniska.FindAsync(id);

        var associatedFlights = await _context.Loty.AnyAsync(l => l.Id_Lotnisko_Wylot == id || l.Id_Lotnisko_Przylot == id);
        if (associatedFlights)
        {
            ModelState.AddModelError("", "Nie można usunąć lotniska, ponieważ istnieją przypisane do niego loty.");
            return RedirectToAction("BladUsuwania" ,"LotniskoUsun");
        }

        _context.Lotniska.Remove(lotnisko);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "LotniskoIndex");
    }

    [HttpGet]
    public IActionResult BladUsuwania()
    {
        return View();
    }
}
