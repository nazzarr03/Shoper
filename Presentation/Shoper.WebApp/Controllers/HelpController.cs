using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.HelpDtos;
using Shoper.Application.Usecasess.HelpServices;

namespace Shoper.WebApp.Controllers;

public class HelpController : Controller
{
    private readonly IHelpService _service;

    public HelpController(IHelpService helpService)
    {
        _service = helpService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateHelp(CreateHelpDto dto)
    {
        dto.CreatedDate = DateTime.Now;
        dto.Status = 0;
        await _service.CreateHelpAsync(dto);
        return RedirectToAction("Index");
    }
}