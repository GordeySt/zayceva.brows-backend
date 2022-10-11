using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class AppUser : IdentityUser<Guid>, IBaseEntity
{
    public IList<AppUserRole> UserRoles { get; set; }

    public DateTime CreationDate { get; set; }
    
    public bool IsDeleted { get; set; }
}