using Application.ApplicationUsers.DTOs;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;

namespace Application.ApplicationUsers.Queries.GetCurrentUsers;

public record GetCurrentUserQuery() : IRequest<ApplicationResult<UserDto>>;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, ApplicationResult<UserDto>>
{
    private readonly IClaimsService _claimsService;
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public GetCurrentUserQueryHandler(IClaimsService claimsService, IIdentityService identityService, IMapper mapper)
    {
        _claimsService = claimsService;
        _identityService = identityService;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<UserDto>> 
        Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _identityService.GetUserByIdAsync(_claimsService.UserId);

        return new ApplicationResult<UserDto>(ApplicationResultType.Success, _mapper.Map<UserDto>(user));
    }
}

