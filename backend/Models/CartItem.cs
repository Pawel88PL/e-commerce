namespace MiodOdStaniula.Models
{
    public class AddCartItem
    {
        public Guid CartId { get; set; }
        public int Price { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class AssignCartModel
    {
        public Guid UserId { get; set; }
    }


    public class UpdateCartItemQuantityModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
