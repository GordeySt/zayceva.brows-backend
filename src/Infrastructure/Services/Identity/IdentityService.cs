using Application.ApplicationUsers.Commands.SignupUsers;
using Application.Common.Interfaces;
using Domain;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityResult = Application.Common.Models.IdentityResult;

namespace Infrastructure.Services.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ApplicationDbContext _applicationDbContext;

    public IdentityService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, 
        ApplicationDbContext applicationDbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _applicationDbContext = applicationDbContext;
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
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

        if (user is not null)
            await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<Guid?> GetUserIdByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        return user?.Id;
    }

    public async Task<AppUser> GetUserByEmailAsync(string email)
    {
        return await _applicationDbContext.Users
            .AsNoTracking()
            .Include(user => user.UserRoles)
            .ThenInclude(userRole => userRole.AppRole)
            .SingleOrDefaultAsync(user => user.Email == email);
    }

    public async Task<AppUser> GetUserByIdAsync(Guid id)
    {
        return await _applicationDbContext.Users
            .Include(user => user.Appointments)
            .Include(user => user.UserRoles)
            .ThenInclude(userRole => userRole.AppRole)
            .SingleOrDefaultAsync(user => user.Id == id);
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(Guid userId)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

        if (user is not null)
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);

        return string.Empty;
    }

    public async Task<IdentityResult> ConfirmEmailAsync(Guid? userId, string token)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);
        
        var result = await _userManager.ConfirmEmailAsync(user, token);

        return result.ToApplicationResult();
    }

    public async Task<IdentityResult> CheckPasswordSignInAsync(AppUser user, string password) 
    {
        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

        return result.ToApplicationResult();
    }

    public async Task<IList<string>> GetRolesAsync(AppUser user)
    {
        return await _userManager.GetRolesAsync(user);
    }
}