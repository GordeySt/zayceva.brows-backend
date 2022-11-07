using Application.Common.Interfaces;
using Domain;
using Infrastructure.Persistence;
using Infrastructure.Services.Auth;
using Infrastructure.Services.Email;
using Infrastructure.Services.Identity;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

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

        services
            .AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = identitySettings.JwtSettings.ValidateIssuer,
                    ValidIssuer = identitySettings.JwtSettings.ValidIssuer,

                    ValidateAudience = identitySettings.JwtSettings.ValidateAudience,
                    ValidAudience = identitySettings.JwtSettings.ValidAudience,

                    ValidateLifetime = identitySettings.JwtSettings.ValidateLifeTime,

                    ValidateIssuerSigningKey = identitySettings.JwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = JwtService
                        .CreateSecuritySigningKey(identitySettings.JwtSettings.IssuerSigningKey),
                };
            });

        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<IEmailService>(sp => new EmailService(smtpClientSettings));
        services.AddScoped<IClaimsService, ClaimsService>();
        services.AddScoped<IJwtService>(sp => new JwtService(identitySettings));

        return services;
    }
}