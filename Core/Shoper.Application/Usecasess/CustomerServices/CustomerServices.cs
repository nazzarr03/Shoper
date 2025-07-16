using Shoper.Application.Dtos.CustomerDtos;
using Shoper.Application.Interfaces;
using Shoper.Domain.Entities;

namespace Shoper.Application.Usecasess.CustomerServices;

public class CustomerServices : ICustomerService
{
    private readonly IRepository<Customer> _repository;

    public CustomerServices(IRepository<Customer> repository)
    {
        _repository = repository;
    }
    
    public async Task CreateCustomerAsync(CreateCustomerDto createCustomerDto)
    {
        await _repository.CreateAsync(new Customer
        {
            FirstName = createCustomerDto.FirstName,
            LastName = createCustomerDto.LastName,
            Email = createCustomerDto.Email,
            UserId = createCustomerDto.UserId,
            PhoneNumber = createCustomerDto.PhoneNumber,
        });
    }
    
    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(customer);
    }

    public async Task<List<ResultCustomerDto>> GetAllCustomerAsync()
    {
        var customers = await _repository.GetAllAsync();
        return customers.Select(x => new ResultCustomerDto
        {
            CustomerId = x.CustomerId,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
            UserId = x.UserId,
            PhoneNumber = x.PhoneNumber,
            //Orders = x.Orders,
        }).ToList();
    }

    public async Task<GetByIdCustomerDto> GetByIdCustomerAsync(int id)
    {
        var customer = await _repository.GetByIdAsync(id);
        return new GetByIdCustomerDto
        {
            CustomerId = customer.CustomerId,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            UserId = customer.UserId,
            PhoneNumber = customer.PhoneNumber,
        };
    }
    
    public async Task<GetByIdCustomerDto> GetCustomerByUserId(string userid)
    {
        var customer = await _repository.FirstOrDefaultAsync(x => x.UserId == userid);
        return new GetByIdCustomerDto
        {
            CustomerId = customer.CustomerId,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            UserId = customer.UserId,
            PhoneNumber = customer.PhoneNumber,
            //Orders = values.Orders,
        };
    }

    public async Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
    {
        var customer = await _repository.GetByIdAsync(updateCustomerDto.CustomerId);
        customer.FirstName = updateCustomerDto.FirstName;
        customer.LastName = updateCustomerDto.LastName;
        customer.Email = updateCustomerDto.Email;
        customer.PhoneNumber = updateCustomerDto.PhoneNumber;
        await _repository.UpdateAsync(customer);
    }

    public async Task UpdateNameAndSurname(string userId, string name, string surname)
    {
        var customer = await _repository.FirstOrDefaultAsync(x => x.UserId == userId);
        customer.FirstName = name;
        customer.LastName = surname;
        await _repository.UpdateAsync(customer);
    }
}