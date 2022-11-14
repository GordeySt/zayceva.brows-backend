using System.Text;
using Application.Common.Constants;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Models;
using HandlebarsDotNet;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;

namespace Application.ApplicationUsers.Commands.SignupUsers;

public class SignupUserCommand : IRequest<ApplicationResult>
{
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string Email { get; set; }
  public string PhoneNumber { get; set; }
  public string Password { get; set; }
  public string? Origin { get; set; }
}

public class SignupUserCommandHandler : IRequestHandler<SignupUserCommand, ApplicationResult>
{
    private readonly IIdentityService _identityService;
    private readonly IEmailService _emailService;

    public SignupUserCommandHandler(IIdentityService identityService, IEmailService emailService)
    {
        _identityService = identityService;
        _emailService = emailService;
    }

    public async Task<ApplicationResult> Handle(SignupUserCommand request, CancellationToken cancellationToken)
    {
        if (await _identityService.GetUserByEmailAsync(request.Email) is not null)
        {
            return new ApplicationResult(
                ApplicationResultType.InvalidData, 
                BadRequestExceptionMessageConstants.ExistedUser);
        }

        var (result, userId) = await _identityService.CreateUserAsync(request);

        if (!result.Succeeded)
            return new ApplicationResult(
                ApplicationResultType.InternalError, 
                InternalServerErrorExceptionMessageConstants.ProblemCreatingUser);

        await _identityService.AddUserToRoleAsync(userId, AppRolesConstants.UserRole);

        var token = await _identityService.GenerateEmailConfirmationTokenAsync(userId);
        
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        SendConfirmationEmail(request.Origin!, token, request.Email);

        return new ApplicationResult(ApplicationResultType.Success);
    }
    
    private void SendConfirmationEmail(string origin, string token, string email)
    {
        Task
            .Run(() => HandleUserEmailConfirmationSending(origin, token, email))
            .ConfigureAwait(continueOnCapturedContext: false);
    }
    
    private void HandleUserEmailConfirmationSending(string origin, string token, string email)
    {
        var confirmationUrl = GenerateConfirmationUrl(origin, token, email);

        var emailLayout = GenerateEmailLayout(confirmationUrl);
        const string subject = "Email confirmation";

        _emailService.SendEmailAsync(email, subject, emailLayout);
    }

    private string GenerateConfirmationUrl(string origin, string token, string email) =>
        $"{origin}/confirmed-email?email={email}&token={token}";
    
    private string GenerateEmailLayout(string confirmationUrl)
    {
        var template = Handlebars.Compile(EmailLayoutConstants.ConfirmEmailLayout);
        var templateData = new
        {
            verifyUrl = confirmationUrl
        };

        return template(templateData);
    }
}