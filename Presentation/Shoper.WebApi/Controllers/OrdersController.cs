using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.OrderDtos;
using Shoper.Application.Usecasess.OrderServices;

namespace Shoper.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class OrdersController : ControllerBase
{
    private readonly IOrderService _service;

    public OrdersController(IOrderService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrder()
    {
        var orders = await _service.GetAllOrderAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdOrder(int id)
    {
        var order = await _service.GetByIdOrderAsync(id);
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
    {
        await _service.CreateOrderAsync(dto);
        return Ok("Order created");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateOrder(UpdateOrderDto dto)
    {
        await _service.UpdateOrderAsync(dto);
        return Ok("Order updated");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        await _service.DeleteOrderAsync(id);
        return Ok("Order deleted");
    }
}