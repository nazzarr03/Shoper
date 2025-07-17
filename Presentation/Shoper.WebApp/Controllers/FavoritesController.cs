using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.FavoritesDtos;
using Shoper.Application.Usecasess.AccountServices;
using Shoper.Application.Usecasess.FavoritesServices;

namespace Shoper.WebApp.Controllers;

[Authorize]
public class FavoritesController : Controller
{
    private readonly IFavoritesService _favoritesService;
    private readonly IAccountService _accountService;

    public FavoritesController(IFavoritesService favoritesService, IAccountService accountService)
    {
        _favoritesService = favoritesService;
        _accountService = accountService;
    }
    
    public async Task<IActionResult> Index()
    {
        var userId = await _accountService.GetUserIdAsync(User);
        var favorites = await _favoritesService.GetFavoritesByUserId(userId);
        return View(favorites);
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(int productid)
    {
        try
        {
            var userId = await _accountService.GetUserIdAsync(User);
            var dto = new CreateFavoritesDto()
            {
                CreatedDate = DateTime.Now,
                ProductId = productid,
                UserId = userId
            };
            var check = await _favoritesService.CheckFavoritesByUseridAndProductId(userId,productid);
            if (check)
            {
                await _favoritesService.CreateFavoritesAsync(dto);
                return Json(new
                {
                    success = true 
                    
                });
            }
            else
            {
                return Json(new
                {
                    success = false, error="Product is already exists in favorites" 
                });
            }
               
        }
        catch (Exception e)
        {
            return Json(new
            {
                success=false, error = e.Message
            });
        }
            
    }
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _favoritesService.DeleteFavoritesAsync(id);
            return Json(new
            { 
                success = true
            });
        }
        catch (Exception e)
        {
            return Json(new
            {
                success = false, error = e.Message
            });
        }
    }
}