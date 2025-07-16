using Shoper.Application.Dtos.SubscriberDtos;
using Shoper.Application.Interfaces;
using Shoper.Domain.Entities;

namespace Shoper.Application.Usecasess.SubscriberServices;

public class SubscriberService : ISubscriberService
{
    private readonly IRepository<Subscriber> _repository;

    public SubscriberService(IRepository<Subscriber> repository)
    {
        _repository = repository;
    }
    
    public async Task CreateSubscriber(CreateSubscriberDto dto)
    {
        await _repository.CreateAsync(new Subscriber
        {
            Email = dto.Email,
            Name = dto.Name,
            SubscribeDate = dto.SubcribeDate,
        });
    }
    
    public async Task DeleteSubscriber(int id)
    {
        var subscriber = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(subscriber);
    }

    public async Task<List<ResultSubscriberDto>> GetAllSubscribers()
    {
        var subscribers = await _repository.GetAllAsync();
        return subscribers.Select(x => new ResultSubscriberDto
        {
            Id = x.Id,
            Email = x.Email,
            Name = x.Name,
            SubcribeDate = x.SubscribeDate,
        }).ToList();
    }
    
    public async Task<List<ResultSubscriberDto>> GetByEmailSubscriber(string email)
    {
        var subscribers = await _repository.WhereAsync(x => x.Email == email);
        return subscribers.Select(x => new ResultSubscriberDto
        {
            Id = x.Id,
            Email = x.Email,
            Name = x.Name,
            SubcribeDate = x.SubscribeDate,
        }).ToList();
    }

    public async Task<GetByIdSubscriberDto> GetByIdSubscriber(int id)
    {
        var subscriber = await _repository.GetByIdAsync(id);
        return new GetByIdSubscriberDto
        {
            Id = subscriber.Id,
            Email = subscriber.Email,
            Name = subscriber.Name,
            SubcribeDate = subscriber.SubscribeDate,
        };
    }

    public async Task UpdateSubscriber(UpdateSubscriberDto dto)
    {
        var subscriber = await _repository.GetByIdAsync(dto.Id);
        subscriber.Email = dto.Email;
        subscriber.Name = dto.Name;
        subscriber.SubscribeDate = dto.SubcribeDate;
        await _repository.UpdateAsync(subscriber);
    }
}