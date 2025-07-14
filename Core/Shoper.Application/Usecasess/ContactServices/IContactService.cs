using Shoper.Application.Dtos.ContactDtos;

namespace Shoper.Application.Usecasess.ContactServices;

public interface IContactService
{
    Task<List<ResultContactDto>> GetAllContactAsync();
    Task<GetByIdContactDto> GetByIdContactAsync(int id);
    Task CreateContactAsync(CreateContactDto model);
    Task UpdateContactAsync(UpdateContactDto model);
    Task DeleteContactAsync(int id);
    Task<List<ResultContactDto>> GetAllContactsByEmailAsync( string email);
}