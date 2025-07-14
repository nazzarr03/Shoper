using Shoper.Application.Dtos.EMailDtos;

namespace Shoper.Application.Usecasess.EmailServices;

public interface IEmailService
{
    bool SendEmailAsync(SendEmailDto dto);
}