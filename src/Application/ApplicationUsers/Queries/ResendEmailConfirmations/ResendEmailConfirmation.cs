using System.Text;
using Application.Common.Constants;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Models;
using HandlebarsDotNet;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;

namespace Application.ApplicationUsers.Queries.ResendEmailConfirmations;

public class ResendEmailConfirmationQuery : IRequest<ApplicationResult>
{
    public string Email { get; set; }
    
    public string Origin { get; set; }
}

public class ResendEmailConfirmationQueryHandler : IRequestHandler<ResendEmailConfirmationQuery, ApplicationResult>
{
    private readonly IIdentityService _identityService;
    private readonly IEmailService _emailService;

    public ResendEmailConfirmationQueryHandler(IEmailService emailService, IIdentityService identityService)
    {
        _emailService = emailService;
        _identityService = identityService;
    }

    public async Task<ApplicationResult> Handle(ResendEmailConfirmationQuery request, 
        CancellationToken cancellationToken)
    {
        var userId = await _identityService.GetUserIdByEmailAsync(request.Email);
        
        if (userId is null)
        {
            return new ApplicationResult(ApplicationResultType.NotFound,
                NotFoundExceptionMessageConstants.NotFoundUser);
        }

        var token = await _identityService.GenerateEmailConfirmationTokenAsync(userId.Value);

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