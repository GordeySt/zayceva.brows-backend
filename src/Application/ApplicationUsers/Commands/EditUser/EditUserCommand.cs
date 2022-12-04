using Application.Common.Constants;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ApplicationUsers.Commands.EditUser;

public class EditUserCommand : IRequest<ApplicationResult>,
    IMapFrom<EditUserCommand>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<EditUserCommand, AppUser>();
    }
}

public class EditUserCommandHandler 
    : IRequestHandler<EditUserCommand, ApplicationResult>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    private readonly IClaimsService _claimsService;
    private readonly IIdentityService _identityService;

    public EditUserCommandHandler(
        IApplicationDbContext applicationDbContext, 
        IMapper mapper, 
        IClaimsService claimsService, IIdentityService identityService)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
        _claimsService = claimsService;
        _identityService = identityService;
    }

    public async Task<ApplicationResult> Handle
        (EditUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityService.GetUserByIdAsync(_claimsService.UserId);
        
        if (user is null)
            return new ApplicationResult(ApplicationResultType.NotFound,
                NotFoundExceptionMessageConstants.NotFoundItem);
        
        _mapper.Map(request, user);
        
        var success = await _applicationDbContext.SaveChangesAsync(cancellationToken) > 0;

        return success ? 
            new ApplicationResult(ApplicationResultType.Success) : 
            new ApplicationResult(ApplicationResultType.InternalError);
    }
}