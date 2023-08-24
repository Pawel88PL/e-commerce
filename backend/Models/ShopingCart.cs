
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiodOdStaniula.Models
{
    public class ShopingCart
    {
        [Key]
        public Guid ShopingCartId { get; set; } 
        
        public Guid? CustomerId { get; set; }

        public DateTime CreateCartDate { get; set; }
        
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        public decimal GetTotalValue()
        {
            return CartItems.Sum(item => item.Price * item.Quantity);
        }
    }

    public class CartItem
    {
        public int CartItemId { get; set; }

        public Guid ShopingCartId { get; set; }

        [ForeignKey(nameof(ShopingCartId))]
        public ShopingCart? ShopingCart { get; set; }

        public int ProductId { get; set; }

        public Product? Product { get; set; }
        
        public int Quantity { get; set; }
        
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }
    }
}
