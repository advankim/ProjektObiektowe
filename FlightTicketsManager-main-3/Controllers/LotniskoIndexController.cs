using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektObiektowe.Models;
using System.Threading.Tasks;

public class LotniskoIndexController : Controller
{
    private readonly BiletContext _context;

    public LotniskoIndexController(BiletContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Lotniska.ToListAsync());
    }
}
