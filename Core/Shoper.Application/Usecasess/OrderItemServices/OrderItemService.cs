using Shoper.Application.Dtos.OrderItemDtos;
using Shoper.Application.Interfaces;
using Shoper.Domain.Entities;

namespace Shoper.Application.Usecasess.OrderItemServices;

public class OrderItemService : IOrderItemService
{
    private readonly IRepository<OrderItem> _repository;

    public OrderItemService(IRepository<OrderItem> repository)
    {
        _repository = repository;
    }
    
    public async Task CreateOrderItemAsync(CreateOrderItemDto model)
    {
        await _repository.CreateAsync(new OrderItem
        {
            ProductId = model.ProductId,
            Quantity = model.Quantity,
            TotalPrice = model.TotalPrice
        });
    }
    
    public async Task DeleteOrderItemAsync(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(item);
    }

    public async Task<List<ResultOrderItemDto>> GetAllOrderItemAsync()
    {
        var items = await _repository.GetAllAsync();
        return items.Select(i => new ResultOrderItemDto
        {
            OrderId = i.OrderId,
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            TotalPrice = i.Quantity,
            OrderItemId = i.OrderItemId,
        }).ToList();
    }

    public async Task<GetByIdOrderItemDto> GetByIdOrderItemAsync(int id)
    {
        var foundItem = await _repository.GetByIdAsync(id);
        var item = new GetByIdOrderItemDto
        {
            ProductId = foundItem.ProductId,
            OrderId = foundItem.OrderId,
            Quantity = foundItem.Quantity,
            TotalPrice = foundItem.TotalPrice,
            OrderItemId = foundItem.OrderItemId
        };
        return item;
    }

    public async Task UpdateOrderItemAsync(UpdateOrderItemDto model)
    {
        var foundItem = await _repository.GetByIdAsync(model.OrderItemId);
        foundItem.Quantity = model.Quantity;
        foundItem.TotalPrice = model.TotalPrice;
        foundItem.ProductId = model.ProductId;
        await _repository.UpdateAsync(foundItem);
    }
}