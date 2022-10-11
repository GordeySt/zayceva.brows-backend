using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasQueryFilter(prop => !prop.IsDeleted);

        builder.Property(prop => prop.FirstName)
            .HasMaxLength(30);

        builder.Property(props => props.LastName)
            .HasMaxLength(50);
    }
}