using Shoper.Application.Dtos.CartDtos;

namespace Shoper.Application.Usecasess.CartServices;

public interface ICartService
{
    Task<List<ResultCartDto>> GelAllCartAsync();
    Task<GetByIdCartDto> GetByIdCartAsync(int id);
    Task CreateCartAsync(CreateCartDto model);
    Task UpdateCartAsync(UpdateCartDto model);
    Task DeleteCartAsync(int id);
    Task UpdateTotalAmount(int cartId, decimal totalAmount);
    Task<GetByIdCartDto> GetByUserIdCartAsync (string userId);
    Task<bool> CheckCartAsync(string userId);
    Task<List<AdminCartDto>> GetAllAdminCartAsync();
}