namespace MiodOdStaniula.Models
{
    public class CartViewModel : ProductViewModel
    {
        public List<CartItem>? CartItems { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal ProductCost { get; set; }
        public decimal TotalCost { get; set; }
    }
}
