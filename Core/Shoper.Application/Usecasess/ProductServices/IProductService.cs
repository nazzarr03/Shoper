using Shoper.Application.Dtos.ProductDtos;

namespace Shoper.Application.Usecasess.ProductServices;

public interface IProductService
{
    Task<List<ResultProductDto>> GetAllProductAsync();
    Task<GetByIdProductDto> GetByIdProductAsync(int id);
    Task CreateProductAsync(CreateProductDto model);
    Task UpdateProductAsync(UpdateProductDto model);
    Task DeleteProductAsync(int id);
    Task<List<ResultProductDto>> GetProductTake(int sayi);
    Task<List<ResultProductDto>> GetProductByCategory(int categoryId);
    Task<List<ResultProductDto>> GetProductByPrice(decimal minprice, decimal maxprice);
    Task<List<ResultProductDto>> GetProductBySearch(string search);
}