using Application.ApplicationUsers.Commands.SignupUsers;
using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class IdentityService : IIdentityService
{

    private readonly UserManager<AppUser> _userManager;
    
    public IdentityService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<(Result result, Guid userId)> CreateUserAsync(SignupUserCommand signupUserCommand)
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

    public async Task AddUserToRole(Guid userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user is not null)
            await _userManager.AddToRoleAsync(user, role);
    }
}