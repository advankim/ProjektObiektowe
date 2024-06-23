using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektObiektowe.Models;
using System.Threading.Tasks;

public class LotniskoEdytujController : Controller
{
    private readonly LotniskoContext _context;

    public LotniskoEdytujController(LotniskoContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> EdytujLotnisko(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lotnisko = await _context.Lotniska.FindAsync(id);
        if (lotnisko == null)
        {
            return NotFound();
        }


        var associatedFlights = await _context.Loty.AnyAsync(l => l.Id_Lotnisko_Wylot == id || l.Id_Lotnisko_Przylot == id);
        if (associatedFlights)
        {
            return RedirectToAction("BladEdycji");
        }

        return View(lotnisko);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EdytujLotnisko(int id, [Bind("Id,Nazwa,Miasto,Kraj")] Lotnisko lotnisko)
    {
        if (id != lotnisko.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {


            _context.Update(lotnisko);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        return View(lotnisko);
    }

    [HttpGet]
    public IActionResult BladEdycji()
    {
        return View();
    }

}
