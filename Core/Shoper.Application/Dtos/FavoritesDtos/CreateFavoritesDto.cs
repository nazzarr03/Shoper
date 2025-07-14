namespace Shoper.Application.Dtos.FavoritesDtos;

public class CreateFavoritesDto
{
    public DateTime CreatedDate { get; set; }
    public int ProductId { get; set; }
    public string UserId { get; set; }
    public int? CustomerId { get; set; }
}