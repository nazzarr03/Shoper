using Shoper.Application.Dtos.FavoritesDtos;

namespace Shoper.Application.Interfaces.IFavoritesRepository;

public interface IFavoritesRepository
{
    Task<List<AdminFavoritesDto>> GetFavoritesGroupUserId();
}