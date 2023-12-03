public class CartItemDto
{
    public int ProductId { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public List<string>? ImagePaths { get; set; }
}

public class CartDto
{
    public Guid ShopingCartId { get; set; }
    public List<CartItemDto>? CartItems { get; set; }
    public decimal TotalValue { get; set; }
}
