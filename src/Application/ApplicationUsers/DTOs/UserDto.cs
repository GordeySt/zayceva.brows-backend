using System.Runtime.InteropServices.ComTypes;
using Application.Common.Mappings;
using AutoMapper;
using Domain;

namespace Application.ApplicationUsers.DTOs;

public class UserDto : IMapFrom<AppUser>
{
    public Guid Id { get; set; }
    
    public string Email { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Role { get; set; }
    
    public string PhoneNumber { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AppUser, UserDto>()
            .ForMember(d => d.Role,
                opts =>
                {
                    opts.MapFrom(u => u.UserRoles.FirstOrDefault().AppRole.Name);
                });
    }
}