using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
{
    public void Configure(EntityTypeBuilder<AppUserRole> builder)
    {
        builder.HasQueryFilter(prop => !prop.IsDeleted);

        builder
            .HasOne(prop => prop.AppRole)
            .WithMany(prop => prop.UserRoles)
            .HasForeignKey(prop => prop.RoleId)
            .HasPrincipalKey(prop => prop.Id)
            .IsRequired();

        builder
            .HasOne(prop => prop.AppUser)
            .WithMany(prop => prop.UserRoles)
            .HasForeignKey(prop => prop.UserId)
            .HasPrincipalKey(prop => prop.Id)
            .IsRequired();
    }
}