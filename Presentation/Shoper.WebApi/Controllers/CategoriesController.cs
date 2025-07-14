using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.CategoryDtos;
using Shoper.Application.Usecasess.CategoryServices;

namespace Shoper.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdCategory(int id)
    {
        var category = await _categoryService.GetByIdCategoryAsync(id);
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryDto dto)
    {
        await _categoryService.CreateCategoryAsync(dto);
        return Ok("Category created");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCategory(UpdateCategoryDto dto)
    {
        await _categoryService.UpdateCategoryAsync(dto);
        return Ok("Category updated");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _categoryService.DeleteCategoryAsync(id);
        return Ok("Category deleted");
    }
}