using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Shoper.Application.Dtos.AccountDtos;
using Shoper.Application.Interfaces;
using Shoper.Persistence.Context.Identity;

namespace Shoper.Persistence.Repositories;

public class UserIdentityRepository : IUserIdentityRepository
{
    private readonly UserManager<AppIdentityUser> _userManager;
    private readonly SignInManager<AppIdentityUser> _signInManager;

    public UserIdentityRepository(
        UserManager<AppIdentityUser> userManager,
        SignInManager<AppIdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<string> ChangePasswordAsync(ChangePasswordDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId);
        if (user == null)
        {
            return "User not found";
        }

        if (dto.NewPassword != dto.ConfirmNewPassword)
        {
            return "Passwords do not match";
        }
        
        var result = await _userManager.ChangePasswordAsync(user, dto.Password, dto.NewPassword);

        if (result.Succeeded)
        {
            return "Password changed successfully";
        }
        
        var errors = string.Join(", ", result.Errors.Select(x => x.Description));
        return $"Password change failed: {errors}";
    }
    
    public async Task<string> GetUserIdOnAuth(ClaimsPrincipal user)
    {
        string userId = _userManager.GetUserId(user);
        if (userId == null)
        {
            userId = "1111111111111";
        }
        return userId;
    }
    
    public async Task<bool> IsUserAuthenticated()
    {
        throw new NotImplementedException();
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            return "User not found";
        }
        
        var result = await _signInManager.PasswordSignInAsync(dto.Email,dto.Password,true,false);
        if (result.Succeeded)
        {
            return "success";
        }

        if (result.IsLockedOut)
        {
            return "locked out";
        }

        if (result.IsNotAllowed)
        {
            return "not allowed";
        }

        if (result.RequiresTwoFactor)
        {
            return "2fa enabled";
        }
        
        return "login failed";
    }
    
    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<string> RegisterAsync(RegisterDto dto)
    {
        if (dto.Password != dto.RePassword)
        {
            return "Passwords do not match";
        }

        var user = new AppIdentityUser
        {
            Name = dto.Name,
            SurName = dto.Surname,
            UserName = dto.Email,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
        };
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (result.Succeeded)
        {
            var result2 = await _signInManager.PasswordSignInAsync(dto.Email,dto.Password,true,false);
            if (result2.Succeeded)
            {
                return user.Id;
            }
            return "register succeeded but login failed";
        }
        else
        {
            return result.Errors.ToString();
        }
    }

    public async Task<bool> UpdateUserNameAndSurnameAsync(string userId, string newName, string newSurname)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }
        
        user.Name = newName;
        user.SurName = newSurname;
        
        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }
}