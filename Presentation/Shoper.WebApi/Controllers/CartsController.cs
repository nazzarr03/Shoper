using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.CartDtos;
using Shoper.Application.Usecasess.CartServices;

namespace Shoper.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class CartsController : ControllerBase
{
    private readonly ICartService _services;

    public CartsController(ICartService services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCart()
    {
        var carts = await _services.GelAllCartAsync();
        return Ok(carts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdCart(int id)
    {
        var cart = await _services.GetByIdCartAsync(id);
        return Ok(cart);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCart(CreateCartDto dto)
    {
        await _services.CreateCartAsync(dto);
        return Ok("Cart created");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCart(UpdateCartDto dto)
    {
        await _services.UpdateCartAsync(dto);
        return Ok("Cart updated");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCart(int id)
    {
        await _services.DeleteCartAsync(id);
        return Ok("Cart deleted");
    }
}