using Shoper.Application.Dtos.HelpDtos;
using Shoper.Application.Interfaces;
using Shoper.Domain.Entities;

namespace Shoper.Application.Usecasess.HelpServices;

public class HelpService : IHelpService
{
    private readonly IRepository<Help> _repository;

    public HelpService(IRepository<Help> repository)
    {
        _repository = repository;
    }

    public async Task CreateHelpAsync(CreateHelpDto model)
    {
        await _repository.CreateAsync(new Help
        {
            Name = model.Name,
            Email = model.Email,
            Subject = model.Subject,
            Message = model.Message,
            CreatedDate = model.CreatedDate,
            Status = model.Status,
        });
    }
    
    public async Task DeleteHelpAsync(int id)
    {
        var help = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(help);
    }
    
    public async Task<List<ResultHelpDto>> GetAllHelpAsync()
    {
        var helps = await _repository.GetAllAsync();
        return helps.Select(x => new ResultHelpDto
        {
            Id = x.Id,
            Name = x.Name,
            Email = x.Email,
            Subject = x.Subject,
            Message = x.Message,
            CreatedDate = x.CreatedDate,
            Status = x.Status,
        }).ToList();
    }
    
    public async Task<List<ResultHelpDto>> GetByEmailHelpAsync(string email)
    {
        var helps = await _repository.WhereAsync(x => x.Email == email);
        return helps.Select(x => new ResultHelpDto
        {
            Id = x.Id,
            Name = x.Name,
            Email = x.Email,
            Subject = x.Subject,
            Message = x.Message,
            CreatedDate = x.CreatedDate,
            Status = x.Status,
        }).ToList();
    }

    public async Task<GetByIdHelpDto> GetByIdHelpAsync(int id)
    {
        var help = await _repository.GetByIdAsync(id);
        var newHelp = new GetByIdHelpDto
        {
            Id = help.Id,
            Name = help.Name,
            Email = help.Email,
            Subject = help.Subject,
            Message = help.Message,
            CreatedDate = help.CreatedDate,
            Status = help.Status,
        };
        
        return newHelp;
    }

    public async Task UpdateHelpAsync(UpdateHelpDto model)
    {
        var help = await _repository.GetByIdAsync(model.Id);
        help.Name = model.Name;
        help.Email = model.Email;
        help.Subject = model.Subject;
        help.Message = model.Message;
        help.Status = model.Status;
        help.CreatedDate = model.CreatedDate;
        await _repository.UpdateAsync(help);
    }
}