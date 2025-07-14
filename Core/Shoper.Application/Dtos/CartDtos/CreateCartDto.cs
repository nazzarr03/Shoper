using Shoper.Application.Dtos.CartItemDtos;
using Shoper.Domain.Entities;

namespace Shoper.Application.Dtos.CartDtos;

public class CreateCartDto
{
    //public decimal TotalAmount { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CustomerId { get; set; }
    //public Customer? Customer { get; set; }
    public ICollection<CreateCartItemDto> CartItems { get; set; }
    public string? UserId { get; set; }
}