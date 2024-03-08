using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Speisekarte.Models
{
    public class Zutat
    {
        public int? Id { get; set; }

        [MaxLength(5), MinLength(2, ErrorMessage = "Minimum Length must be 2")]
        public string Beschreibung { get; set; } = string.Empty;

        public string Einheit { get; set; } = string.Empty;
        [Required]
        public decimal Menge { get; set; }
        public Speise? Speise { get; set; }
        public int? SpeiseId { get; set; }

        public override string ToString()
        {
            return $"{Menge} {Einheit} {Beschreibung}";
        }
    }
}
