using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Infrastructure.Settings;
using WebApi;
using WebApi.Settings;

var builder = WebApplication.CreateBuilder(args);

var dbSettings = builder.Configuration
    .GetSection(nameof(AppSettings.DbSettings))
    .Get<DbSettings>();

var appSettings = new AppSettings
{
    DbSettings = dbSettings
};

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(appSettings.DbSettings);
builder.Services.AddWebApiServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Initialise and seed database
    using var scope = app.Services.CreateScope();
    
    var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
    await initialiser.InitialiseAsync();
    await initialiser.SeedAsync();
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
