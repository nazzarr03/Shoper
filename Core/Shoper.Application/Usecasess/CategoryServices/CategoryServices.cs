using Shoper.Application.Dtos.CategoryDtos;
using Shoper.Application.Interfaces;
using Shoper.Domain.Entities;

namespace Shoper.Application.Usecasess.CategoryServices;

public class CategoryServices : ICategoryService
{
    private readonly IRepository<Category> _repository;

    public CategoryServices(IRepository<Category> repository)
    {
        _repository = repository;
    }
    
    public async Task CreateCategoryAsync(CreateCategoryDto model)
    {
        await _repository.CreateAsync(new Category
        {
            CategoryName = model.CategoryName,
        });
    }
    
    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(category);
    }

    public async Task<List<ResultCategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _repository.GetAllAsync();
        return categories.Select(c => new ResultCategoryDto
        {
            CategoryId = c.CategoryId,
            CategoryName = c.CategoryName,
        }).ToList();
    }

    public async Task<GetByIdCategoryDto> GetByIdCategoryAsync(int id)
    {
        var findCategory = await _repository.GetByIdAsync(id);
        var category = new GetByIdCategoryDto
        {
            CategoryId = findCategory.CategoryId,
            CategoryName = findCategory.CategoryName,
        };
        return category;
    }
    
    public async Task UpdateCategoryAsync(UpdateCategoryDto model)
    {
        var findCategory = await _repository.GetByIdAsync(model.CategoryId);
        findCategory.CategoryName = model.CategoryName;
        await _repository.UpdateAsync(findCategory);
    }
}