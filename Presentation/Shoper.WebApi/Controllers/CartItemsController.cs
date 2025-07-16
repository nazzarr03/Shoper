using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.CartItemDtos;
using Shoper.Application.Usecasess.CartItemSevices;
using Shoper.Domain.Entities;

namespace Shoper.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class CartItemsController : ControllerBase
{
    private readonly ICartItemService _services;

    public CartItemsController(ICartItemService services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCartItems()
    {
        var cartItems = await _services.GetAllCartItemAsync();
        return Ok(cartItems);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdCartItem(int id)
    {
        var cartItem = await _services.GetByIdCartItemAsync(id);
        return Ok(cartItem);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCartItem(CreateCartItemDto dto)
    {
        await _services.CreateCartItemAsync(dto);
        return Ok("Cart Item created");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCartItemAsync(UpdateCartItemDto dto)
    {
        await _services.UpdateCartItemAsync(dto);
        return Ok("Cart Item updated");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCartItem(int id)
    {
        await _services.DeleteCartItemAsync(id);
        return Ok("Cart Item deleted");
    }
}