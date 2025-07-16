using System.Linq.Expressions;
using Shoper.Domain.Entities;

namespace Shoper.Application.Interfaces.IOrderRepository;

public interface IOrdersRepository
{
    Task<List<City>> GetCity();
    Task<List<Town>> GetTownByCityId(int cityid);
}