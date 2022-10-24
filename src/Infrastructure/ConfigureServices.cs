using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Services.Email;
using Infrastructure.Services.Identity;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        DbSettings dbSettings, IdentitySettings identitySettings, SmtpClientSettings smtpClientSettings)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(dbSettings.ConnectionString,
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        
        services.AddScoped<ApplicationDbContextInitialiser>();
        
        services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequireDigit = identitySettings.PasswordSettings.RequireDigit;
                options.Password.RequireLowercase = identitySettings.PasswordSettings.RequireLowercase;
                options.Password.RequireUppercase = identitySettings.PasswordSettings.RequireUppercase;
                options.Password.RequireNonAlphanumeric = identitySettings.PasswordSettings.RequireNonAlphanumeric;
                options.Password.RequiredUniqueChars = identitySettings.PasswordSettings.RequiredUniqueChars;
            })
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<IEmailService>(sp => new EmailService(smtpClientSettings));

        return services;
    }
}