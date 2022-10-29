using FluentValidation;

namespace Application.Common.Validators;

public static class FluentValidatorExtensions
{
    public static void Password<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty().WithMessage("Password must not be empty")
            .MinimumLength(7).WithMessage("Password length must be greater than 6");
    }

    public static void Email<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty().WithMessage("Email must not be empty")
            .EmailAddress().WithMessage("Invalid Email address");
    }
    
    public static void FirstName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty().WithMessage("First name must not be empty")
            .MaximumLength(30).WithMessage("First name length must be less than 30");
    }
    
    public static void LastName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty().WithMessage("Last name must not be empty")
            .MaximumLength(50).WithMessage("Last name length must be less than 50");
    }
    
    public static void PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty().WithMessage("Phone number must not be empty")
            .WithMessage("Phone number format is invalid");
    }
}