namespace Shoper.Application.Dtos.ContactDtos;

public class CreateContactDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
    public DateTime CreatedDate { get; set; }
}