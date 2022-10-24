using Application.ApplicationUsers.Commands.SignupUsers;
using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<(IdentityResult result, Guid userId)> CreateUserAsync(SignupUserCommand command);

    Task AddUserToRoleAsync(Guid userId, string role);

    Task<Guid?> GetUserIdByEmailAsync(string email);

    Task<string> GenerateEmailConfirmationTokenAsync(Guid userId);
}