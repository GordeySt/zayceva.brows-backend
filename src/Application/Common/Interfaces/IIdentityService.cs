using Application.ApplicationUsers.Commands.SignupUsers;
using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<(Result result, Guid userId)> CreateUserAsync(SignupUserCommand command);

    Task AddUserToRole(Guid userId, string role);
}