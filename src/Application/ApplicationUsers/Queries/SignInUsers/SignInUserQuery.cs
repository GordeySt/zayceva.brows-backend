using Application.ApplicationUsers.DTOs;
using Application.ApplicationUsers.Models;
using Application.Common.Constants;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;

namespace Application.ApplicationUsers.Queries.SignInUsers;

public record SignInUserQuery(string Email, string Password) : IRequest<ApplicationResult<SignInResponse>>;

public class SignInUserQueryHandler : IRequestHandler<SignInUserQuery, ApplicationResult<SignInResponse>>
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;
    private readonly IClaimsService _claimsService;
    private readonly IJwtService _jwtService;

    public SignInUserQueryHandler(
        IIdentityService identityService,
        IMapper mapper, 
        IClaimsService claimsService,
        IJwtService jwtService)
    {
        _identityService = identityService;
        _mapper = mapper;
        _claimsService = claimsService;
        _jwtService = jwtService;
    }

    public async Task<ApplicationResult<SignInResponse>> 
        Handle(SignInUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _identityService.GetUserByEmailAsync(request.Email);

        if (user is null)
        {
            return new ApplicationResult<SignInResponse>(ApplicationResultType.Unauthorized,
                NotFoundExceptionMessageConstants.NotFoundItem);
        }

        var result = await _identityService.CheckPasswordSignInAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return new ApplicationResult<SignInResponse>(ApplicationResultType.Unauthorized,
                UnauthorizedExceptionMessageConstants.InvalidPassword);
        }
        
        var roles = await _identityService.GetRolesAsync(user);
        var authUser = _claimsService.AssignClaims(user.Id, user.Email, roles.FirstOrDefault());
        var token = _jwtService.GenerateJwtToken(authUser.Claims);

        var signInResponse = new SignInResponse
        {
            Jwt = token,
            User = _mapper.Map<UserDto>(user)
        };

        return new ApplicationResult<SignInResponse>(ApplicationResultType.Success, signInResponse);
    }
}
