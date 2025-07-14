using Shoper.Application.Dtos.CustomerDtos;

namespace Shoper.Application.Usecasess.CustomerServices;

public interface ICustomerService
{
    Task<List<ResultCustomerDto>> GetAllCustomerAsync();
    Task<GetByIdCustomerDto> GetByIdCustomerAsync(int id);
    Task CreateCustomerAsync(CreateCustomerDto createCustomerDto);
    Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto);
    Task DeleteCustomerAsync(int id);
    Task<GetByIdCustomerDto> GetCustomerByUserId(string userid);
    Task UpdateNameAndSurname(string userId, string name, string surname);
}