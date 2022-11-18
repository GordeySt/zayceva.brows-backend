using Microsoft.AspNetCore.Identity;

namespace Domain;

public class AppUser : IdentityUser<Guid>, IBaseIdentityEntity
{
    public IList<AppUserRole> UserRoles { get; set; }
    
    public IList<Appointment> Appointments { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.Now.ToUniversalTime();

    public bool IsDeleted { get; set; }
}