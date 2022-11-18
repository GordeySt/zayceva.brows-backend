using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Appointments.Commands.CreateAppointments;

public class CreateAppointmentCommand : IRequest<ApplicationResult<CreateAppointmentCommand>>,
    IMapFrom<CreateAppointmentCommand>
{
    public string Title { get; set; }
    
    public ushort Price { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateAppointmentCommand, Appointment>();
    }
}

public class CreateAppointmentCommandHandler 
    : IRequestHandler<CreateAppointmentCommand, ApplicationResult<CreateAppointmentCommand>>
{
    private readonly IApplicationDbContext _applicationDbContext;

    private readonly IMapper _mapper;

    public CreateAppointmentCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<CreateAppointmentCommand>> Handle
        (CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        _applicationDbContext.Appointments
            .Add(_mapper.Map<Appointment>(request));
        
        var success = await _applicationDbContext.SaveChangesAsync(cancellationToken) > 0;

        return success ? 
            new ApplicationResult<CreateAppointmentCommand>(ApplicationResultType.Success, request) : 
            new ApplicationResult<CreateAppointmentCommand>(ApplicationResultType.InternalError);
    }
}


