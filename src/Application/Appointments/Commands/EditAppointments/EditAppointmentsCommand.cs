using Application.Common.Constants;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Appointments.Commands.EditAppointments;

public class EditAppointmentCommand : IRequest<ApplicationResult>,
    IMapFrom<EditAppointmentCommand>
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<EditAppointmentCommand, Appointment>()
            .ForMember(x => x.Id, 
                opts => opts.Ignore());
    }
}

public class EditAppointmentCommandHandler 
    : IRequestHandler<EditAppointmentCommand, ApplicationResult>
{
    private readonly IApplicationDbContext _applicationDbContext;

    private readonly IMapper _mapper;

    public EditAppointmentCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<ApplicationResult> Handle
        (EditAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _applicationDbContext.Appointments
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (appointment is null)
            return new ApplicationResult(ApplicationResultType.NotFound,
                NotFoundExceptionMessageConstants.NotFoundItem);

        _mapper.Map(request, appointment);
        
        var success = await _applicationDbContext.SaveChangesAsync(cancellationToken) > 0;

        return success ? 
            new ApplicationResult(ApplicationResultType.Success) : 
            new ApplicationResult(ApplicationResultType.InternalError);
    }
}