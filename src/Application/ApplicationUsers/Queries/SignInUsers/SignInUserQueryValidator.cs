using Application.Common.Validators;
using FluentValidation;

namespace Application.ApplicationUsers.Queries.SignInUsers;

public class SignInUserQueryValidator : AbstractValidator<SignInUserQuery>
{
    public SignInUserQueryValidator()
    {
        RuleFor(v => v.Email).Email();
        RuleFor(v => v.Password).Password();
    }
}
