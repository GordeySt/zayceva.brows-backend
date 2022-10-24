using Application.Common.Validators;
using FluentValidation;

namespace Application.ApplicationUsers.Queries.ResendEmailConfirmations;

public class ResendEmailConfirmationValidator : AbstractValidator<ResendEmailConfirmationQuery>
{
    public ResendEmailConfirmationValidator()
    {
        RuleFor(v => v.Email).Email();
    }
}