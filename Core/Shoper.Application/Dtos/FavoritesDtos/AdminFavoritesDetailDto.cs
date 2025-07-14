using Shoper.Domain.Entities;

namespace Shoper.Application.Dtos.FavoritesDtos;

public class AdminFavoritesDetailDto
{
    public DateTime CreatedDate { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}