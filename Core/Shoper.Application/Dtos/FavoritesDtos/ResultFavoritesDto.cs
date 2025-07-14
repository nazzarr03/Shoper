using Shoper.Domain.Entities;

namespace Shoper.Application.Dtos.FavoritesDtos;

public class ResultFavoritesDto
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public string UserId { get; set; }
    public int? CustomerId { get; set; }
}