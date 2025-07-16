using Shoper.Application.Dtos.ContactDtos;
using Shoper.Application.Interfaces;
using Shoper.Domain.Entities;

namespace Shoper.Application.Usecasess.ContactServices;

public class ContactService : IContactService
{
    private readonly IRepository<Contact> _repository;

    public ContactService(IRepository<Contact> repository)
    {
        _repository = repository;
    }
    
    public async Task CreateContactAsync(CreateContactDto model)
    {
        await _repository.CreateAsync(new Contact
        {
            Name = model.Name,
            Email = model.Email,
            Message = model.Message,
            CreatedDate = model.CreatedDate,
        });
    }
    
    public async Task DeleteContactAsync(int id)
    {
        var contact = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(contact);
    }

    public async Task<List<ResultContactDto>> GetAllContactAsync()
    {
        var contacts = await _repository.GetAllAsync();
        return contacts.Select(c => new ResultContactDto
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Message = c.Message,
            CreatedDate = c.CreatedDate,
        }).ToList();
    }
    
    public async Task<List<ResultContactDto>> GetAllContactsByEmailAsync(string email)
    {
        var contact = await _repository.WhereAsync(c => c.Email == email);
        return contact.Select(c => new ResultContactDto
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Message = c.Message,
            CreatedDate = c.CreatedDate,
        }).ToList();
    }

    public async Task<GetByIdContactDto> GetByIdContactAsync(int id)
    {
        var contact = await _repository.GetByIdAsync(id);
        var newContact = new GetByIdContactDto
        {
            Id = contact.Id,
            Name = contact.Name,
            Email = contact.Email,
            Message = contact.Message,
            CreatedDate = contact.CreatedDate,
        };
        return newContact;
    }

    public async Task UpdateContactAsync(UpdateContactDto model)
    {
        var contact = await _repository.GetByIdAsync(model.Id);
        contact.Name = model.Name;
        contact.Email = model.Email;
        contact.Message = model.Message;
        contact.CreatedDate = model.CreatedDate;
        await _repository.UpdateAsync(contact);
    }
}