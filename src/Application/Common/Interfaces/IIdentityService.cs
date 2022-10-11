using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<(Result result, Guid userId)> CreateUserAsync(string email, string password, string phoneNumber);

    Task AddUserToRole(Guid userId, string role);
}