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

    public async Task<(Result result, Guid userId)> CreateUserAsync(string email, string password, string phoneNumber)
    {
        var user = new AppUser
        {
            UserName = email,
            Email = email,
            PhoneNumber = phoneNumber
        };

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task AddUserToRole(Guid userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user is not null)
            await _userManager.AddToRoleAsync(user, role);
    }
}