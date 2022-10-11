using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Startup.Settings;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        DbSettings dbSettings)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(dbSettings.ConnectionString,
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        
        services.AddIdentity<AppUser, AppRole>()
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}