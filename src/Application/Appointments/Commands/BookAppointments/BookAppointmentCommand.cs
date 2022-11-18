using Application.Common.Constants;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Appointments.Commands.BookAppointments;

public record BookAppointmentCommand(Guid Id) : IRequest<ApplicationResult>;

public class BookAppointmentCommandHandler 
    : IRequestHandler<BookAppointmentCommand, ApplicationResult>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IIdentityService _identityService;
    private readonly IClaimsService _claimsService;

    public BookAppointmentCommandHandler(
        IApplicationDbContext applicationDbContext, 
        IIdentityService identityService, 
        IClaimsService claimsService)
    {
        _applicationDbContext = applicationDbContext;
        _identityService = identityService;
        _claimsService = claimsService;
    }

    public async Task<ApplicationResult> Handle
        (BookAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _applicationDbContext.Appointments
            .Include(t => t.AppUser)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (appointment is null)
            return new ApplicationResult(ApplicationResultType.NotFound,
                NotFoundExceptionMessageConstants.NotFoundItem);

        if (appointment.AppUser is not null)
            return new ApplicationResult(ApplicationResultType.InvalidData);

        var user = await _identityService.GetUserByIdAsync(_claimsService.UserId);

        BookAppointment(appointment, user);

        var success = await _applicationDbContext.SaveChangesAsync(cancellationToken) > 0;

        return success ? 
            new ApplicationResult(ApplicationResultType.Success) : 
            new ApplicationResult(ApplicationResultType.InternalError);
    }

    private void BookAppointment(Appointment appointment, AppUser user)
    {
        appointment.IsBooked = true;
        appointment.UserId = user.Id;
    }
}