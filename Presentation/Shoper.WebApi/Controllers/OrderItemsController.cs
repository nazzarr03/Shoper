using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.OrderItemDtos;
using Shoper.Application.Usecasess.OrderItemServices;
using Shoper.Domain.Entities;

namespace Shoper.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class OrderItemsController : ControllerBase
{
    private readonly IOrderItemService _orderItemService;

    public OrderItemsController(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrderItem()
    {
        var items = await _orderItemService.GetAllOrderItemAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdOrderItem(int id)
    {
        var item = await _orderItemService.GetByIdOrderItemAsync(id);
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrderItem(CreateOrderItemDto dto)
    {
        await _orderItemService.CreateOrderItemAsync(dto);
        return Ok("Order Item created");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateOrderItem(UpdateOrderItemDto dto)
    {
        await _orderItemService.UpdateOrderItemAsync(dto);
        return Ok("Order Item updated");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteOrderItem(int id)
    {
        await _orderItemService.DeleteOrderItemAsync(id);
        return Ok("Order Item deleted");
    }
}