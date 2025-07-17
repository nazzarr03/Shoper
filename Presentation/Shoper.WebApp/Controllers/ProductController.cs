using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Usecasess.ProductServices;

namespace Shoper.WebApp.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index(int categoryId, decimal minPrice, decimal maxPrice, string search, int pageNumber = 1, int pageSize = 6)
    {
        if (categoryId != 0)
        {
            var products = await _productService.GetProductByCategory(categoryId);
            
            // Sayfa numarası ve boyutuna göre ürünleri parçala sadece o sayfanınkileri al
            var pageProducts = products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            
            int totalProducts = products.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;
            
            return View(pageProducts);
        }

        if (maxPrice != 0)
        {
            var products = await _productService.GetProductByPrice(minPrice, maxPrice);
            var pageProduct1 = products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            int totalProducts1 = products.Count();
            int totalPages1 = (int)Math.Ceiling((double)totalProducts1 / pageSize);
            
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages1;
            
            return View(pageProduct1);
        }

        var allProducts = await _productService.GetAllProductAsync();
        var pageProduct = allProducts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        int totalProduct = allProducts.Count();
        int totalPage = (int)Math.Ceiling((double)totalProduct / pageSize);
        
        ViewBag.PageNumber = pageNumber;
        ViewBag.TotalPages = totalPage;
        
        return View(pageProduct);
    }

    [HttpPost]
    public async Task<IActionResult> Index(string search)
    {
        if (search == null)
        {
            return View();
        }
        
        var value = await _productService.GetProductBySearch(search);
        return View(value);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var product = await _productService.GetByIdProductAsync(id);
        return View(product);
    }
}