namespace Shoper.Application.Dtos.CartItemDtos;

public class GetByIdCartItemDto
{
    public int CartItemId { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    //public Product Products{get: set:}
    public int Quantity { get; set; }
    public int TotalPrice { get; set; }
}