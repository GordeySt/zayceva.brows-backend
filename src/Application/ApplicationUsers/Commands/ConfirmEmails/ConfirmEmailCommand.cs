using System.Text;
using Application.Common.Constants;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;

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
        var user = await _identityService.GetUserByEmailAsync(request.Email);

        if (user is null)
        {
            return new ApplicationResult(ApplicationResultType.NotFound,
                NotFoundExceptionMessageConstants.NotFoundItem);
        }

        var decodedTokenBytes = WebEncoders.Base64UrlDecode(request.Token);
        var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);
        
        var result = await _identityService.ConfirmEmailAsync(user.Id, decodedToken);

        if (!result.Succeeded)
        {
            return new ApplicationResult(ApplicationResultType.InvalidData, 
                BadRequestExceptionMessageConstants.ProblemVerifyingEmail);
        }

        return new ApplicationResult(ApplicationResultType.Success);
    }
}

