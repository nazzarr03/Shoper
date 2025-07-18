using Shoper.Application.Dtos.CartItemDtos;

namespace Shoper.Application.Usecasess.CartItemSevices;

public interface ICartItemService
{
    Task<List<ResultCartItemDto>> GetAllCartItemAsync();
    Task<GetByIdCartItemDto> GetByIdCartItemAsync(int id);
    Task CreateCartItemAsync(CreateCartItemDto model);
    Task UpdateCartItemAsync(UpdateCartItemDto model);
    Task DeleteCartItemAsync(int id);
    Task<List<ResultCartItemDto>> GetByCartIdCartItemAsync(int cartId);
    Task UpdateQuantity(int cartId, int productId, int quantity);
    Task UpdateQuantityOnCart(UpdateCartItemDto dto);
    Task<bool> CheckCartItems(int cartId, int productId);
    Task<int> GetCountCartItemsByCartId(string userId);
}