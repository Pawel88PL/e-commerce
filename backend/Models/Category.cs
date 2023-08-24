using System.ComponentModel.DataAnnotations;

namespace MiodOdStaniula.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Podaj nazwę kategorii")]
        [StringLength(30, ErrorMessage = "Nazwa kategorii nie może przekraczać 30 znaków")]
        public string? Name { get; set; }

        [StringLength(500, ErrorMessage = "Opis kategorii nie może przekraczać 500 znaków")]
        public string? Description { get; set; }
    }
}
