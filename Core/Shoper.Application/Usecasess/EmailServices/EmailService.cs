using System.Net;
using System.Net.Mail;
using Shoper.Application.Dtos.EMailDtos;

namespace Shoper.Application.Usecasess.EmailServices;

public class EmailService : IEmailService
{
    public bool SendEmailAsync(SendEmailDto dto)
    {
        try
        {
            MailMessage message = new MailMessage(
                dto.SenderEmail,
                dto.ReciverEmail,
                dto.Subject,
                dto.Message);
            
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(dto.SenderEmail, dto.SenderPassword);
            smtpClient.EnableSsl = true;
            smtpClient.Send(message);
            return true;

        }
        catch (Exception)
        {
            return false;
        }
    }
}