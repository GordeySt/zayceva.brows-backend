using Application.ApplicationUsers.Commands.SignupUsers;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using IdentityResult = Application.Common.Models.IdentityResult;

namespace Infrastructure.Services.Identity;

public class IdentityService : IIdentityService
{

    private readonly UserManager<AppUser> _userManager;
    
    public IdentityService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<(IdentityResult result, Guid userId)> CreateUserAsync(SignupUserCommand signupUserCommand)
    {
        var user = new AppUser
        {
            FirstName = signupUserCommand.FirstName,
            LastName = signupUserCommand.LastName,
            UserName = signupUserCommand.Email,
            Email = signupUserCommand.Email,
            PhoneNumber = signupUserCommand.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, signupUserCommand.Password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task AddUserToRoleAsync(Guid userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user is not null)
            await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<Guid?> GetUserIdByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        return user?.Id;
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(Guid userId)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user is not null)
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);

        return string.Empty;
    }

    public async Task<IdentityResult> ConfirmEmailAsync(Guid? userId, string token)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);
        
        var result = await _userManager.ConfirmEmailAsync(user, token);

        return result.ToApplicationResult();
    }
}