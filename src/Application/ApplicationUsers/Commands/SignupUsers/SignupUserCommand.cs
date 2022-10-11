using Application.Common.Interfaces;
using Application.Constants;
using MediatR;

namespace Application.ApplicationUsers.Commands.SignupUsers;

public record SignupUserCommand(string Email, string PhoneNumber, string Password) : IRequest;

public class SignupUserCommandHandler : IRequestHandler<SignupUserCommand>
{
    private readonly IIdentityService _identityService;

    public SignupUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Unit> Handle(SignupUserCommand request, CancellationToken cancellationToken)
    {
        var (result, userId) = await _identityService.CreateUserAsync(
            request.Email, 
            request.Password, 
            request.PhoneNumber);

        if (!result.Succeeded) 
            throw new Exception("Problem creating user");

        await _identityService.AddUserToRole(userId, AppRolesConstants.UserRole);
        
        return Unit.Value;
    }
}