using Shoper.Application.Dtos.FavoritesDtos;

namespace Shoper.Application.Usecasess.FavoritesServices;

public interface IFavoritesService
{
    Task<List<ResultFavoritesDto>> GetAllFavoritesAsync();
    Task<GetByIdFavoritesDto> GetByIdFavoritesAsync(int id);
    Task CreateFavoritesAsync(CreateFavoritesDto createFavoritesDto);
    Task UpdateFavoritesAsync(UpdateFavoritesDto updateFavoritesDto);
    Task DeleteFavoritesAsync(int id);
    Task<List<ResultFavoritesDto>> GetFavoritesByUserId(string userid);
    Task<bool> CheckFavoritesByUseridAndProductId(string userId, int productId);
    Task<int> GetCountByUserId(string userid);
    Task<List<AdminFavoritesDto>> GetAdminFavoritesList();
}