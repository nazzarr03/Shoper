using Shoper.Application.Dtos.CustomerDtos;
using Shoper.Application.Interfaces;
using Shoper.Domain.Entities;

namespace Shoper.Application.Usecasess.CustomerServices;

public class CustomerServices : ICustomerServices
{
    private readonly IRepository<Customer> _customerRepository;

    public CustomerServices(IRepository<Customer> customerRepository)
    {
        _customerRepository = customerRepository;
    }


    public async Task<List<ResultCustomerDto>> GetAllCustomerAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        var result = customers.Select(x => new ResultCustomerDto
        {
            CustomerId = x.CustomerId,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
            UserId = x.UserId,
            PhoneNumber = x.PhoneNumber,
        }).ToList();
        return result;
    }

    public async Task<GetByIdCustomerDto> GetByIdCustomerAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        var result = 
    }

    public Task CreateCustomerAsync(CreateCustomerDto createCustomerDto)
    {
        throw new NotImplementedException();
    }

    public Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCustomerAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<GetByIdCustomerDto> GetCustomerByUserId(string userid)
    {
        throw new NotImplementedException();
    }

    public Task UpdateNameAndSurname(string userId, string name, string surname)
    {
        throw new NotImplementedException();
    }
}