using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder
            .HasOne(a => a.AppUser)
            .WithMany(au => au.Appointments)
            .HasForeignKey(a => a.UserId)
            .HasPrincipalKey(au => au.Id);
    }
}