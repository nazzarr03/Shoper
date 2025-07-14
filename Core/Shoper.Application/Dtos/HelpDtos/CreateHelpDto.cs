namespace Shoper.Application.Dtos.HelpDtos;

public class CreateHelpDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public DateTime CreatedDate { get; set; }
    public int Status { get; set; }
}