using Application;
using Infrastructure;
using Infrastructure.Persistence;
using WebApi;
using WebApi.Settings;

var builder = WebApplication.CreateBuilder(args);

var appSettings = AppSettingsBuilder.ReadAppSettings(builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(
    appSettings.DbSettings, 
    appSettings.IdentitySettings, 
    appSettings.SmtpClientSettings);
builder.Services.AddWebApiServices(builder.Configuration);

builder.Services.AddSingleton(appSettings);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    
    using var scope = app.Services.CreateScope();

    var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
    await initialiser.InitialiseAsync();
    await initialiser.SeedAsync();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ZaycevaBrowsApi v1");
    options.RoutePrefix = string.Empty;
});

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
