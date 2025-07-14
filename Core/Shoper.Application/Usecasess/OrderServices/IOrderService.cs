using Shoper.Application.Dtos.CityDtos;
using Shoper.Application.Dtos.DashboardDtos;
using Shoper.Application.Dtos.OrderDtos;
using Shoper.Application.Dtos.TownDtos;

namespace Shoper.Application.Usecasess.OrderServices;

public interface IOrderService
{
    Task<List<ResultOrderDto>> GetAllOrderAsync();
    Task<GetByIdOrderDto> GetByIdOrderAsync(int id);
    Task CreateOrderAsync(CreateOrderDto model);
    Task UpdateOrderAsync(UpdateOrderDto model);
    Task DeleteOrderAsync(int id);
    Task<List<ResultCityDto>> GetAllCity();
    Task<List<ResultTownDto>> GetTownByCityId(int cityId);
    Task<List<ResultOrderDto>> GetOrderByUserId(string userId);
    Task UpdateOrderStatus(int orderId,string orderstatus);
    Task<List<SalesWithCategory>> GetOrderByCategory();
    Task<List<DashboardSoledProductDto>> GetSoledProducts();
    Task<List<DashboardOrderStatusDto>> GetOrderStatusGraphs();
    Task<List<SalesTrendDto>> GetSalesTrends();
    Task<DashboardCardsDto> GetDashboardCards();
}