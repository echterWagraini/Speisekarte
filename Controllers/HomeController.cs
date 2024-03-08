using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Speisekarte.Data;
using Speisekarte.Models;
using System.Diagnostics;

namespace Speisekarte.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var speisen = _context.Speisen.Include(speise => speise.Zutaten).ToList();
            return View(speisen);
        }

        public IActionResult CreateSpeiseForm()
        {
            //// Informationen f�r eine Drop-Down-Liste bereitstellen
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            SelectListItem eintrag1 = new SelectListItem("Yummy Good", "5");
            selectListItems.Add(eintrag1);
            SelectListItem eintrag2 = new SelectListItem("Good", "4");
            selectListItems.Add(eintrag2);
            SelectListItem eintrag3 = new SelectListItem("Nice", "3");
            selectListItems.Add(eintrag3);
            SelectListItem eintrag4 = new SelectListItem("Ok", "2");
            selectListItems.Add(eintrag4);
            SelectListItem eintrag5 = new SelectListItem("Awful", "1");
            selectListItems.Add(eintrag5);

            // Die �bergabe erfolgt �ber das ViewData-Dictionary
            ViewData["Kategorien"] = selectListItems;

            return View();
        }


        public IActionResult CreateSpeise(Speise speise)
        {
            _context.Speisen.Add(speise);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        // Zeige das Formular an
        public IActionResult AddZutatForm(int? id)
        {
            if (id is not null && id > 0)
            {
                var speisenFromDB = _context.Speisen.FirstOrDefault(s => s.Id == id);
                if (speisenFromDB is not null)
                {
                    Zutat zutat = new Zutat();
                    zutat.SpeiseId = id;
                    return View(zutat);
                }
            }
            return RedirectToAction(nameof(Index));
        }


        public IActionResult AddZutat(Zutat zutat)
        {
            if (zutat.SpeiseId is not null && zutat.SpeiseId > 0)
            {
                Speise? speisenFromDB = _context.Speisen.FirstOrDefault(x => x.Id == zutat.SpeiseId);
                if (speisenFromDB is not null)
                {
                    speisenFromDB.Zutaten.Add(zutat);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}
