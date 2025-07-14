using Shoper.Domain.Entities;

namespace Shoper.Application.Interfaces.IProductsRepository;

public interface IProductsRepository
{
    Task<List<Product>> GetProductByCategory(int categoryId);
    Task<List<Product>> GetProductByPriceFilter(decimal minPrice, decimal maxPrice);
    Task<List<Product>> GetProductBySearch(string search);
}