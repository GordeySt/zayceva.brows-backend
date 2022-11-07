using Application.ApplicationUsers.Commands.SignupUsers;
using Application.Common.Models;
using Domain;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<(IdentityResult result, Guid userId)> CreateUserAsync(SignupUserCommand command);

    Task AddUserToRoleAsync(Guid userId, string role);

    Task<Guid?> GetUserIdByEmailAsync(string email);

    Task<AppUser> GetUserByEmailAsync(string email);

    Task<string> GenerateEmailConfirmationTokenAsync(Guid userId);

    Task<IdentityResult> ConfirmEmailAsync(Guid? userId, string token);

    Task<IdentityResult> CheckPasswordSignInAsync(AppUser user, string password);

    Task<IList<string>> GetRolesAsync(AppUser user);
}