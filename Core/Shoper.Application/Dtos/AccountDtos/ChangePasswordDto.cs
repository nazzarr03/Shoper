namespace Shoper.Application.Dtos.AccountDtos;

public class ChangePasswordDto
{
    public string UserId { get; set; }
    public string Password { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmNewPassword { get; set; }
}