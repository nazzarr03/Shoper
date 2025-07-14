namespace Shoper.Application.Dtos.FavoritesDtos;

public class AdminFavoritesDto
{
    public string UserId { get; set; }
    public string NameSurname { get; set; }
    public List<AdminFavoritesDetailDto> FavoritesDetails { get; set; }
}