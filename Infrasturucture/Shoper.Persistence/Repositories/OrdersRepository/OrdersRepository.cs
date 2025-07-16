using Microsoft.EntityFrameworkCore;
using Shoper.Application.Interfaces.IOrderRepository;
using Shoper.Domain.Entities;
using Shoper.Persistence.Context;

namespace Shoper.Persistence.Repositories.OrdersRepository;

public class OrdersRepository : IOrdersRepository
{
    private readonly AppDbContext _context;

    public OrdersRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<City>> GetCity()
    {
        var cities = await _context.City.ToListAsync();
        return cities;
    }

    public async Task<List<Town>> GetTownByCityId(int cityid)
    {
        var town = await _context.Town.Where(x => x.CityId == cityid).ToListAsync();
        return town;
    }
}