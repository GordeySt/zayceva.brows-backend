using Infrastructure;
using Microsoft.Extensions.DependencyInjection.Startup.Settings;

var builder = WebApplication.CreateBuilder(args);

var dbSettings = builder.Configuration
    .GetSection(nameof(AppSettings.DbSettings))
    .Get<DbSettings>();

var appSettings = new AppSettings
{
    DbSettings = dbSettings
};

builder.Services.UseConfigurationValidation();
builder.Services.ConfigureValidatableSetting<AppSettings>(builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(appSettings.DbSettings);
builder.Services.AddWebApiServices();

var app = builder.Build();

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
