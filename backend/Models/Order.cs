using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiodOdStaniula.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public virtual UserModel User { get; set; } = null!;
        [Required]
        public DateTime OrderDate { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal TotalPrice { get; set; }
        public string? Status { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }

    public class OrderDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public List<OrderDetailDTO> OrderDetails { get; set; } = new List<OrderDetailDTO>();
    }


    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal UnitPrice { get; set;}
    }

    public class OrderDetailDTO
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; } // Opcjonalnie, jeśli chcesz referencję do Order w szczegółach
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}