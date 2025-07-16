namespace Shoper.Application.Interfaces.ICartsRepository;

public interface ICartsRepository
{
    Task UpdateTotalAmountAsync(int cartId, decimal totalPrice);

}