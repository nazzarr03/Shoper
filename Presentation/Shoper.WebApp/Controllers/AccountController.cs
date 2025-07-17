using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.AccountDtos;
using Shoper.Application.Dtos.CustomerDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Usecasess.AccountServices;
using Shoper.Application.Usecasess.CustomerServices;
using Shoper.Application.Usecasess.OrderItemServices;
using Shoper.Application.Usecasess.OrderServices;

namespace Shoper.WebApp.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly ICustomerService _customerService;
    private readonly IUserIdentityRepository _userIdentityRepository;
    private readonly IOrderService _orderService;
    private readonly IOrderItemService _orderItemService;

    public AccountController(
        IAccountService accountService,
        ICustomerService customerService,
        IUserIdentityRepository userIdentityRepository,
        IOrderService orderService,
        IOrderItemService orderItemService)
    {
        _accountService = accountService;
        _customerService = customerService;
        _userIdentityRepository = userIdentityRepository;
        _orderService = orderService;
        _orderItemService = orderItemService;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _accountService.Login(dto);
        return RedirectToAction("Index", "Home");
    }
    
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var result = await _accountService.Register(dto);
        var customer = new CreateCustomerDto
        {
            Email = dto.Email,
            FirstName = dto.Name,
            LastName = dto.Surname,
            UserId = result,
            PhoneNumber = dto.PhoneNumber,
        };
        await _customerService.CreateCustomerAsync(customer);
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await _accountService.Logout();
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Profile()
    {
        var userID = await _userIdentityRepository.GetUserIdOnAuth(User);
        var user = await _customerService.GetCustomerByUserId(userID);
        var orders = await _orderService.GetOrderByUserId(userID);
        var result = new ResultProfileDto
        {
            Email = user.Email,
            Name = user.FirstName,
            Surname = user.LastName,
            PhoneNumber = user.PhoneNumber,
            Orders = orders,
            UserId = userID,
        };
        return View(result);
    }

    [HttpPost]
    public async Task<IActionResult> Profile(string name, string surname)
    {
        var userID = await _accountService.GetUserIdAsync(User);
        var result = await _accountService.UpdateUser(userID, name, surname);
        await _customerService.UpdateNameAndSurname(userID, name, surname);
        return RedirectToAction("Profile", "Account");
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        var userId = await _accountService.GetUserIdAsync(User);
        dto.UserId = userId;
        var result = await _accountService.ChangePassword(dto);
        return RedirectToAction("Profile", "Account");
    }
}