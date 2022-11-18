using Application.Common.Constants;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Appointments.Commands.DeleteAppointments;

public record DeleteAppointmentCommand(Guid Id) : IRequest<ApplicationResult>;

public class DeleteAppointmentCommandHandler 
    : IRequestHandler<DeleteAppointmentCommand, ApplicationResult>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public DeleteAppointmentCommandHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<ApplicationResult> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _applicationDbContext.Appointments
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (appointment is null)
            return new ApplicationResult(ApplicationResultType.NotFound,
                NotFoundExceptionMessageConstants.NotFoundItem);

        _applicationDbContext.Appointments.Remove(appointment);
        
        var success = await _applicationDbContext.SaveChangesAsync(cancellationToken) > 0;

        return success ? 
            new ApplicationResult(ApplicationResultType.Success) : 
            new ApplicationResult(ApplicationResultType.InternalError);
    }
}