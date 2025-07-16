using Shoper.Application.Dtos.CartItemDtos;

namespace Shoper.Application.Interfaces.ICartItemsRepository;

public interface ICartItemsRepository
{
    Task UpdateQuantity(int cartId, int productId, int quantity);
    Task UpdateQuantityOnCartAsync(UpdateCartItemDto dto);
    Task<bool> CheckCartItemAsync(int cartId, int productId);
}