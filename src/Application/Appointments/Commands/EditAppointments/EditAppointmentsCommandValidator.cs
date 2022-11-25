using FluentValidation;

namespace Application.Appointments.Commands.EditAppointments;

public class EditAppointmentCommandValidator : AbstractValidator<EditAppointmentCommand>
{
    public EditAppointmentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        
        RuleFor(x => x.Title).NotEmpty();

        RuleFor(x => x.StartDate).NotEmpty();

        RuleFor(x => x.EndDate).NotEmpty();
        
        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .WithMessage("Start date must be less than end date");
    }
}