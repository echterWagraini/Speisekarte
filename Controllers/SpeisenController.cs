using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Speisekarte.Data;
using Speisekarte.Models;

namespace Speisekarte.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeisenController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SpeisenController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Fill")]
        public void FillData()
        {
            Speise s1 = new Speise { Titel = "Schweinsbraten", Notizen = "Sooooo guad!", Sterne = 4 };
            Zutat z1 = new Zutat { Beschreibung = "Schweinas", Einheit = "g", Menge = 1000 };
            Zutat z2 = new Zutat { Beschreibung = "Sauce", Einheit = "g", Menge = 100 };
            Zutat z3 = new Zutat { Beschreibung = "Semeelknödel", Einheit = "g", Menge = 1000 };

            s1.Zutaten.Add(z1);
            s1.Zutaten.Add(z2);
            s1.Zutaten.Add(z3);

            _context.Speisen.Add(s1);
            _context.SaveChanges();
        }


        [HttpGet]
        [Route("GetAll")]
        public List<Speise> GetSpeisen()
        {
            var speisen = _context.Speisen
                .Include(speise => speise.Zutaten) // Includiert die Zutaten der Klasse
                .ToList();
            return speisen;
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null ||_context.Speisen == null)
            {
                return NotFound();
            }

            var speise = await _context.Speisen
                .Include(speise => speise.Zutaten)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (speise == null)
            {
                return NotFound();
            }
            else
            {

                //var zutaten = await _context.Zutaten.Where(z => z.SpeiseId == id).ToListAsync();
                // Alle Zutaten der Speise löschen
                foreach (var zutat in speise.Zutaten)
                {
                    _context.Zutaten.Remove(zutat);
                }
            }

            _context.Speisen.Remove(speise);

            await _context.SaveChangesAsync();

            return NoContent();

        }
        [HttpGet]
        [Route("GetTopSpeisen")]
        public IActionResult GetTopSpeisen()
        {
            // Get the top 3 rated speisen based on Sterne
            var top3Speisen = _context.Speisen.OrderByDescending(s => s.Sterne.GetValueOrDefault(0))
                                     .Take(3)
                                     .Select(s => new SpeiseViewModel
                                     {
                                         Titel = s.Titel,
                                         Sterne = s.Sterne
                                     })
                                     .ToList();

            return Ok(top3Speisen);
        }
        public class SpeiseViewModel
        {
            public string Titel { get; set; }
            public int? Sterne { get; set; }
        }
    }
}
