using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Usecasess.AccountServices;
using Shoper.Application.Usecasess.FavoritesServices;

namespace Shoper.WebApp.ViewComponents;

public class FavoriteSummaryViewComponent : ViewComponent
{
    private readonly IAccountService _accountService;
    private readonly IFavoritesService _favoritesService;

    public FavoriteSummaryViewComponent(IAccountService accountService, IFavoritesService favoritesService)
    {
        _accountService = accountService;
        _favoritesService = favoritesService;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        int favoriteItemCount = 0;
        if (User.Identity.IsAuthenticated)
        {
            var userId = await _accountService.GetUserIdAsync(UserClaimsPrincipal);
            var favoriteCount = await _favoritesService.GetCountByUserId(userId);
            if (favoriteCount == 0)
            {
                favoriteItemCount = 0;
            }
            else
            {
                favoriteItemCount = favoriteCount;
            }
        }
        
        return View(favoriteItemCount);
    }
}