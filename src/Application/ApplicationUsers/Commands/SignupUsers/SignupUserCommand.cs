using Application.Common.Constants;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.ApplicationUsers.Commands.SignupUsers;

public record SignupUserCommand(
    string FirstName, string LastName, string Email, string PhoneNumber, string Password) : IRequest<ApplicationResult>;

public class SignupUserCommandHandler : IRequestHandler<SignupUserCommand, ApplicationResult>
{
    private readonly IIdentityService _identityService;

    public SignupUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ApplicationResult> Handle(SignupUserCommand request, CancellationToken cancellationToken)
    {
        if (await _identityService.GetUserIdByEmailAsync(request.Email) is not null)
        {
            return new ApplicationResult(
                ApplicationResultType.InvalidData, 
                BadRequestExceptionMessageConstants.ExistedUserMessage);
        }

        var (result, userId) = await _identityService.CreateUserAsync(request);

        if (!result.Succeeded)
            return new ApplicationResult(
                ApplicationResultType.InternalError, 
                InternalServerErrorExceptionMessageConstants.ProblemCreatingUser);

        await _identityService.AddUserToRoleAsync(userId, AppRolesConstants.UserRole);
        
        return new ApplicationResult(ApplicationResultType.Success);
    }
}