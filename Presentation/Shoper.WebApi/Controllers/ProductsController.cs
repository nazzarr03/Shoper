using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.ProductDtos;
using Shoper.Application.Usecasess.ProductServices;

namespace Shoper.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProduct()
    {
        var products = await _productService.GetAllProductAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdProduct(int id)
    {
        var product = await _productService.GetByIdProductAsync(id);
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductDto dto)
    {
        await _productService.CreateProductAsync(dto);
        return Ok("Product created");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct(UpdateProductDto dto)
    {
        await _productService.UpdateProductAsync(dto);
        return Ok("Product updated");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _productService.DeleteProductAsync(id);
        return Ok("Product deleted");
    }
}