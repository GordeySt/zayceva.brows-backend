using Microsoft.AspNetCore.Identity;

namespace Domain;

public class AppRole : IdentityRole<Guid>, IBaseIdentityEntity
{
    public IList<AppUserRole> UserRoles { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.Now.ToUniversalTime();

    public bool IsDeleted { get; set; }
}