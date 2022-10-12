using Application.Common.Validators;
using FluentValidation;

namespace Application.ApplicationUsers.Commands.SignupUsers;

public class SignupUserCommandValidator : AbstractValidator<SignupUserCommand>
{
    public SignupUserCommandValidator()
    {
        RuleFor(v => v.Email).Email();
        RuleFor(v => v.Password).Password();
        RuleFor(v => v.FirstName).FirstName();
        RuleFor(v => v.LastName).LastName();
        RuleFor(v => v.PhoneNumber).PhoneNumber();
    }
}