using Shoper.Application.Dtos.OrderDtos;

namespace Shoper.Application.Dtos.AccountDtos;

public class ResultProfileDto
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public List<ResultOrderDto> Orders { get; set; }
}