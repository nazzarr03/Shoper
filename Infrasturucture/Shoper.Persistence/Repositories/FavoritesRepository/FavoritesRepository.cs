using Microsoft.EntityFrameworkCore;
using Shoper.Application.Dtos.FavoritesDtos;
using Shoper.Application.Interfaces.IFavoritesRepository;
using Shoper.Domain.Entities;
using Shoper.Persistence.Context;

namespace Shoper.Persistence.Repositories.FavoritesRepository;

public class FavoritesRepository : IFavoritesRepository
{
    private readonly AppDbContext _context;

    public FavoritesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AdminFavoritesDto>> GetFavoritesGroupUserId()
    {
        var favorites = await _context.Favorites.GroupBy(f => f.UserId).Select(x => new AdminFavoritesDto
        {
            UserId = x.Key,
            NameSurname = _context.Customers
                .Where(c => c.UserId == x.Key)
                .Select(a => a.FirstName + " " + a.LastName)
                .FirstOrDefault(),
            FavoritesDetails = x.Select(z => new AdminFavoritesDetailDto
            {
                CreatedDate = z.CreatedDate,
                ProductId = z.ProductId,
                Product = _context.Products.
                    Where(p => p.ProductId == z.ProductId)
                    .Select(n => new Product
                {
                    ProductName = n.ProductName,
                }).FirstOrDefault()
            }).ToList()
        }).ToListAsync();
        
        return favorites;
    }
}