namespace Shoper.Domain.Entities;

public class Favorites
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public int ProductId { get; set; }
    public string UserId { get; set; }
    public int? CustomerId { get; set; }
}