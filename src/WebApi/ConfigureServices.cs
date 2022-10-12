using FluentValidation.AspNetCore;
using WebApi.Filters;
using WebApi.Settings;

namespace WebApi;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks();
        services
            .AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>());

        services.AddFluentValidationClientsideAdapters();

        services.UseConfigurationValidation();
        services.ConfigureValidatableSetting<AppSettings>(configuration);
        
        services.AddSwaggerGen();

        return services;
    }
}