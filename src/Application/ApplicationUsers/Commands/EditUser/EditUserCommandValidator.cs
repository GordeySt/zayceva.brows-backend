using FluentValidation;

namespace Application.ApplicationUsers.Commands.EditUser;

public class EditUserCommandValidator : AbstractValidator<EditUserCommand>
{
    public EditUserCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();

        RuleFor(x => x.LastName).NotEmpty();

        RuleFor(x => x.PhoneNumber).NotEmpty();
    }
}