using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using WebApi.Filters;
using WebApi.Settings;

namespace WebApi;

public static class ConfigureServices
{
    public static void AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks();
        services
            .AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>());

        services.AddFluentValidationClientsideAdapters();

        services.UseConfigurationValidation();
        services.ConfigureValidatableSetting<AppSettings>(configuration);

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ZaycevaBrowsApi",
                Version = "v1",
                Description = "Api for managing beauty salon",
                Contact = new OpenApiContact()
                {
                    Name = "Elizabeth",
                    Email = "zayceva.brows@gmail.com"
                }
            });
                
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            c.IncludeXmlComments(xmlPath);
        });
    }
}