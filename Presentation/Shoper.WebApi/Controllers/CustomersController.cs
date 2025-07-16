using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.CustomerDtos;
using Shoper.Application.Usecasess.CustomerServices;
using Shoper.Persistence.Context;

namespace Shoper.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class CustomersController : ControllerBase
{
    private readonly ICustomerService _services;
    private readonly AppDbContext _dbContext;

    public CustomersController(
        ICustomerService services,
        AppDbContext dbContext)
    {
        _services = services;
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomer()
    {
        var customers = await _services.GetAllCustomerAsync();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdCustomer(int id)
    {
        var customer = await _services.GetByIdCustomerAsync(id);
        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer(CreateCustomerDto dto)
    {
        await _services.CreateCustomerAsync(dto);
        return Ok("Customer created");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCustomer(UpdateCustomerDto dto)
    {
        await _services.UpdateCustomerAsync(dto);
        return Ok("Customer updated");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        await _services.DeleteCustomerAsync(id);
        return Ok("Customer deleted");
    }
}