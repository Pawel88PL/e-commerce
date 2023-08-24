using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace MiodOdStaniula.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Priorytetem określasz kolejność wyświetlania produktu na stronie")]
        public int Priority { get; set; }

        [Required(ErrorMessage = "Podaj nazwę produktu")]
        [StringLength(100, ErrorMessage = "Nazwa produktu nie może przekraczać 100 znaków.")]
        public string? Name { get; set; }

        [StringLength(2000, ErrorMessage = "Opis produktu nie może przekraczać 2000 znaków.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Podaj cenę produktu")]
        [Column(TypeName = "decimal(6, 2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Podaj wagę produktu")]
        public int Weight { get; set; }

        public int AmountAvailable { get; set; }

        public int? Popularity { get; set; }

        public DateTime DateAdded { get; set; }

        public ICollection<ProductImage>? ProductImages { get; set; }

        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }



        // Categories  
        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

    }
}
