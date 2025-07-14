using Shoper.Application.Dtos.HelpDtos;

namespace Shoper.Application.Usecasess.HelpServices;

public interface IHelpService
{
    Task<List<ResultHelpDto>> GetAllHelpAsync();
    Task<GetByIdHelpDto> GetByIdHelpAsync(int id);
    Task CreateHelpAsync(CreateHelpDto model);
    Task UpdateHelpAsync(UpdateHelpDto model);
    Task DeleteHelpAsync(int id);
    Task<List<ResultHelpDto>> GetByEmailHelpAsync(string email);
}