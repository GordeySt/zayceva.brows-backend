using Application.Appointments.DTOs;
using Application.Appointments.Queries.Params;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Appointments.Queries.GetAppointments;

public record GetAppointmentsQuery(AppointmentParams Params) : IRequest<ApplicationResult<List<AppointmentDto>>>;

public class GetAppointmentsQueryHandler 
    : IRequestHandler<GetAppointmentsQuery, ApplicationResult<List<AppointmentDto>>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetAppointmentsQueryHandler(IMapper mapper, IApplicationDbContext applicationDbContext)
    {
        _mapper = mapper;
        _applicationDbContext = applicationDbContext;
    }

    public async Task<ApplicationResult<List<AppointmentDto>>> 
        Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var appointments = _applicationDbContext.Appointments
            .AsNoTracking()
            .ProjectTo<AppointmentDto>(_mapper.ConfigurationProvider)
            .AsQueryable();

        if (request.Params.StartDate is not null && request.Params.EndDate is not null)
        {
            appointments = appointments.Where(x =>
                x.StartDate >= request.Params.StartDate && x.EndDate <= request.Params.EndDate);
        }

        return new ApplicationResult<List<AppointmentDto>>
            (ApplicationResultType.Success, await appointments.ToListAsync(cancellationToken));
    }
}