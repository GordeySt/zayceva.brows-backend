using Application.Appointments.DTOs;
using Application.Appointments.Queries.Params;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Appointments.Queries.GetCurrentUserBookedAppointments;

public record GetCurrentUserBookedAppointmentsQuery() : IRequest<ApplicationResult<List<AppointmentDto>>>;

public class GetCurrentUserBookedAppointmentsQueryHandler 
    : IRequestHandler<GetCurrentUserBookedAppointmentsQuery, ApplicationResult<List<AppointmentDto>>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    private readonly IClaimsService _claimsService;

    public GetCurrentUserBookedAppointmentsQueryHandler(
        IMapper mapper,
        IApplicationDbContext applicationDbContext,
        IClaimsService claimsService)
    {
        _mapper = mapper;
        _applicationDbContext = applicationDbContext;
        _claimsService = claimsService;
    }

    public async Task<ApplicationResult<List<AppointmentDto>>> 
        Handle(GetCurrentUserBookedAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var appointments = await _applicationDbContext.Appointments
            .AsNoTracking()
            .Include(x => x.AppUser)
            .Where(x => x.IsBooked && x.AppUser.Id == _claimsService.UserId)
            .ProjectTo<AppointmentDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new ApplicationResult<List<AppointmentDto>>
            (ApplicationResultType.Success, appointments);
    }
}