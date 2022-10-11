using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class AppUserRole : IdentityUserRole<Guid>
{
    public AppUser AppUser { get; set; }
    
    public AppRole AppRole { get; set; }
    
    public bool IsDeleted { get; set; }
}