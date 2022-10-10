namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        services.AddHealthChecks();
        services.AddControllers();
        
        services.AddSwaggerGen();

        return services;
    }
}