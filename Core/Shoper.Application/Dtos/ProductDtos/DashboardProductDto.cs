using Shoper.Application.Dtos.CategoryDtos;

namespace Shoper.Application.Dtos.ProductDtos;

public class DashboardProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public GetByIdCategoryDto Category { get; set; }
}