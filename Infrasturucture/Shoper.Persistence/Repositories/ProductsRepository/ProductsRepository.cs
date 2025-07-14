using Microsoft.EntityFrameworkCore;
using Shoper.Application.Interfaces.IProductsRepository;
using Shoper.Domain.Entities;
using Shoper.Persistence.Context;

namespace Shoper.Persistence.Repositories.ProductsRepository;

public class ProductsRepository : IProductsRepository
{
    private readonly AppDbContext _context;

    public ProductsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetProductByCategory(int categoryId)
    {
        return await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
    }

    public async Task<List<Product>> GetProductByPriceFilter(decimal minPrice, decimal maxPrice)
    {
        return await _context.Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToListAsync();
    }

    public async Task<List<Product>> GetProductBySearch(string search)
    {
        return await _context.Products.Where(p => p.ProductName.Contains(search) || p.Description.Contains(search)).ToListAsync();
    }
}