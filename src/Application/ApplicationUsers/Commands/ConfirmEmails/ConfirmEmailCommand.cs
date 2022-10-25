using Application.Common.Constants;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.ApplicationUsers.Commands.ConfirmEmails;

public class ConfirmEmailCommand : IRequest<ApplicationResult>
{
    public string Email { get; set; }
    public string Token { get; set; }
}

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ApplicationResult>
{
    private readonly IIdentityService _identityService;

    public ConfirmEmailCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ApplicationResult> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var userId = await _identityService.GetUserIdByEmailAsync(request.Email);

        if (userId is null)
        {
            return new ApplicationResult(ApplicationResultType.NotFound,
                NotFoundExceptionMessageConstants.NotFoundUserMessage);
        }

        var result = await _identityService.ConfirmEmailAsync(userId, request.Token);

        if (!result.Succeeded)
        {
            return new ApplicationResult(ApplicationResultType.InvalidData, 
                BadRequestExceptionMessageConstants.ProblemVerifyingEmailMessage);
        }

        return new ApplicationResult(ApplicationResultType.Success);
    }
}

