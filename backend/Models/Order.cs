using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiodOdStaniula.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public Guid? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        public decimal TotalPrice { get; set; }

        public string? Status { get; set; }

        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
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
}
