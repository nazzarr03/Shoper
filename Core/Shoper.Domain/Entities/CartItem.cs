namespace Shoper.Domain.Entities;

public class CartItem
{
    public int CartItemId { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public Product Products { get; set; }
    public int Quantity { get; set; }
    public int TotalPrice { get; set; }
}