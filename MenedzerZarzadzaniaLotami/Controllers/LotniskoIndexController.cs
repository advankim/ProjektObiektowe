using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektObiektowe.Models;
using System.Linq;
using System.Threading.Tasks;

public class LotniskoIndexController : Controller
{
    private readonly BiletContext _context;

    public LotniskoIndexController(BiletContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string sortOrderMiasto, string sortOrderKraj, string currentFilterMiasto, string currentFilterKraj, string searchStringMiasto, string searchStringKraj)
    {
        ViewData["CurrentSortMiasto"] = sortOrderMiasto;
        ViewData["CurrentSortKraj"] = sortOrderKraj;
        ViewData["MiastoSortParam"] = string.IsNullOrEmpty(sortOrderMiasto) ? "miasto_desc" : "";
        ViewData["KrajSortParam"] = sortOrderKraj == "kraj" ? "kraj_desc" : "kraj";



        ViewData["CurrentFilterMiasto"] = searchStringMiasto;
        ViewData["CurrentFilterKraj"] = searchStringKraj;

        var lotniska = from l in _context.Lotniska
                       select l;

        if (!string.IsNullOrEmpty(searchStringMiasto))
        {
            lotniska = lotniska.Where(l => l.Miasto.Contains(searchStringMiasto));
        }

        if (!string.IsNullOrEmpty(searchStringKraj))
        {
            lotniska = lotniska.Where(l => l.Kraj.Contains(searchStringKraj));
        }

        switch (sortOrderMiasto)
        {
            case "miasto_desc":
                lotniska = lotniska.OrderByDescending(l => l.Miasto);
                break;
            case "miasto":
                lotniska = lotniska.OrderBy(l => l.Miasto);
                break;
            default:
                lotniska = lotniska.OrderBy(l => l.Miasto);
                break;
        }

        switch (sortOrderKraj)
        {
            case "kraj_desc":
                lotniska = lotniska.OrderByDescending(l => l.Kraj);
                break;
            case "kraj":
                lotniska = lotniska.OrderBy(l => l.Kraj);
                break;
            default:
                lotniska = lotniska.OrderBy(l => l.Kraj);
                break;
        }

        return View(await lotniska.AsNoTracking().ToListAsync());
    }
}