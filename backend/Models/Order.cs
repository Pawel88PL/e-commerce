using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public virtual UserModel User { get; set; } = null!;
        [Required]
        public DateTime OrderDate { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal TotalPrice { get; set; }
        public string? Status { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TransactionId { get; set; }
        public bool IsPickupInStore { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }

    public class OrderDTO
    {
        public Guid OrderId { get; set; }
        public string ShortOrderId { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsPickupInStore { get; set; }        
        public virtual UserDto Customer { get; set; } = null!;
        public List<OrderDetailDTO> OrderDetails { get; set; } = new List<OrderDetailDTO>();
    }


    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }
        [Required]
        public Guid OrderId { get; set; }
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
        public Guid OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductName { get; set; } = string.Empty;
    }

    public class AdminOrderDTO
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsPickupInStore { get; set; }
    }

    public class OrderHistoryDTO
    {
        public string OrderId { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsPickupInStore { get; set; }
        public string Status { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}