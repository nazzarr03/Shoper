namespace Shoper.Application.Dtos.OrderItemDtos;

public class UpdateOrderItemDto
{
    public int OrderItemId { get; set; }
    //public int OrderId { get; set; }
    //public Order Order { get; set; }
    public int ProductId { get; set; }
    //public Product Product { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}