namespace Shoper.Application.Dtos.ContactDtos;

public class UpdateContactDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
    public DateTime CreatedDate { get; set; }
}