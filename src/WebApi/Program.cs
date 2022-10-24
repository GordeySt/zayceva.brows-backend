using Application;
using Infrastructure;
using Infrastructure.Persistence;
using WebApi;
using WebApi.Settings;

var builder = WebApplication.CreateBuilder(args);

var appSettings = AppSettingsBuilder.ReadAppSettings(builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(appSettings.DbSettings, appSettings.IdentitySettings);
builder.Services.AddWebApiServices(builder.Configuration);

builder.Services.AddSingleton(appSettings);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

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
