using Shoper.Application.Dtos.SubscriberDtos;

namespace Shoper.Application.Usecasess.SubscriberServices;

public interface ISubscriberService
{
    Task<List<ResultSubscriberDto>> GetAllSubscribers();
    Task<GetByIdSubscriberDto> GetByIdSubscriber(int id);
    Task CreateSubscriber(CreateSubscriberDto dto);
    Task UpdateSubscriber(UpdateSubscriberDto dto);
    Task DeleteSubscriber(int id);
    Task<List<ResultSubscriberDto>> GetByEmailSubscriber(string email);
}