using Shoper.Application.Dtos.ProductDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Interfaces.IProductsRepository;
using Shoper.Domain.Entities;

namespace Shoper.Application.Usecasess.ProductServices;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _repository;
    private readonly IProductsRepository _productsRepository;

    public ProductService(IRepository<Product> repository, IProductsRepository productsRepository)
    {
        _repository = repository;
        _productsRepository = productsRepository;
    }
    
    public async Task CreateProductAsync(CreateProductDto model)
    {
        await _repository.CreateAsync(new Product
        {
            ProductName = model.ProductName,
            Description = model.Description,
            Price = model.Price,
            Stock = model.Stock,
            ImageUrl = model.ImageUrl,
            CategoryId = model.CategoryId
        });
    }
    
    public async Task DeleteProductAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(product);
    }

    public async Task<List<ResultProductDto>> GetAllProductAsync()
    {
        var products = await _repository.GetAllAsync();
        return products.Select(x => new ResultProductDto
        {
            ProductId = x.ProductId,
            ProductName = x.ProductName,
            Description = x.Description,
            Price = x.Price,
            Stock = x.Stock,
            ImageUrl = x.ImageUrl,
            CategoryId = x.CategoryId
        }).ToList();
    }

    public async Task<GetByIdProductDto> GetByIdProductAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        var result = new GetByIdProductDto
        {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            ImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId
        };
        return result;
    }
    
    public async Task<List<ResultProductDto>> GetProductByCategory(int categoryId)
    {
        var products = await _productsRepository.GetProductByCategory(categoryId);
        return products.Select(x => new ResultProductDto
        {
            ProductId = x.ProductId,
            ProductName = x.ProductName,
            Description = x.Description,
            Price = x.Price,
            Stock = x.Stock,
            ImageUrl = x.ImageUrl,
            CategoryId = x.CategoryId
        }).ToList();
    }

    public async Task<List<ResultProductDto>> GetProductByPrice(decimal minPrice, decimal maxPrice)
    {
        var products = await _productsRepository.GetProductByPriceFilter(minPrice, maxPrice);
        return products.Select(x => new ResultProductDto
        {
            ProductId = x.ProductId,
            ProductName = x.ProductName,
            Description = x.Description,
            Price = x.Price,
            Stock = x.Stock,
            ImageUrl = x.ImageUrl,
            CategoryId = x.CategoryId
        }).ToList();
    }
    
    public async Task<List<ResultProductDto>> GetProductBySearch(string search)
    {
        var products = await _productsRepository.GetProductBySearch(search);
        return products.Select(x => new ResultProductDto
        {
            ProductId = x.ProductId,
            ProductName = x.ProductName,
            Description = x.Description,
            Price = x.Price,
            Stock = x.Stock,
            ImageUrl = x.ImageUrl,
            CategoryId = x.CategoryId
        }).ToList();
    }
    
    public async Task<List<ResultProductDto>> GetProductTake(int sayi)
    {
        var products = await _repository.GetTakeAsync(sayi);
        return products.Select(x => new ResultProductDto
        {
            ProductId = x.ProductId,
            ProductName = x.ProductName,
            Description = x.Description,
            Price = x.Price,
            Stock = x.Stock,
            ImageUrl = x.ImageUrl,
            CategoryId = x.CategoryId
        }).ToList();
    }
    
    public async Task UpdateProductAsync(UpdateProductDto model)
    {
        var product = await _repository.GetByIdAsync(model.ProductId);
        product.ProductName = model.ProductName;
        product.Description = model.Description;
        product.Price = model.Price;
        product.Stock = model.Stock;
        product.ImageUrl = model.ImageUrl;
        product.CategoryId = model.CategoryId;
        await _repository.UpdateAsync(product);
    }
}