namespace Shoper.Application.Dtos.HelpDtos;

public class GetByIdHelpDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public DateTime CreatedDate { get; set; }
    public int Status { get; set; }
}