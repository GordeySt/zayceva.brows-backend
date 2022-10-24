using Application.Common.Validators;
using FluentValidation;

namespace Application.ApplicationUsers.Commands.ConfirmEmails;

public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(v => v.Email).Email();
        RuleFor(v => v.Token).NotEmpty().WithMessage("Token must not be empty");
    }
}