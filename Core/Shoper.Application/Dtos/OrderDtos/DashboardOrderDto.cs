using Shoper.Application.Dtos.OrderItemDtos;

namespace Shoper.Application.Dtos.OrderDtos;

public class DashboardOrderDto
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string OrderStatus { get; set; }
    //public string BillingAdress { get; set; }
    public string ShippingAdress { get; set; }
    public int ShippingCityId { get; set; }
    public int ShippingTownId { get; set; }
    //public string PaymentMethod { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerSurname { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerPhone { get; set; }
    public List<DashboardOrderItemDto> OrderItems { get; set; }
    public string UserId { get; set; }
}