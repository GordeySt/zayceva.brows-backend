using System.Linq.Expressions;
using Application.Common.Constants;
using Infrastructure.Persistence;
using Infrastructure.Services.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using NUnit.Framework;
using Respawn;
using Table = Respawn.Graph.Table;

namespace Application.IntegrationTests;

[SetUpFixture]
public partial class Testing
{
    private static WebApplicationFactory<Program> _factory = null!;
    private static IConfiguration _configuration = null!;
    private static IServiceScopeFactory _scopeFactory = null!;
    private static Respawner _respawner = null!;

    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        _factory = new CustomWebApplicationFactory();
        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
        _configuration = _factory.Services.GetRequiredService<IConfiguration>();


        await using var connection = 
            new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        
        await connection.OpenAsync();
        
        _respawner = await Respawner.CreateAsync(
            connection,
            new RespawnerOptions
            {
                TablesToIgnore = new Table[]
                {
                    new("__EFMigrationsHistory")
                },
                DbAdapter = DbAdapter.Postgres
            });
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public static async Task ResetState()
    {
        await using var connection = 
            new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        
        await connection.OpenAsync();
        
        await _respawner.ResetAsync(connection);
    }

    public static async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }
    
    public static async Task<TEntity?> SingleOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> expression)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().SingleOrDefaultAsync(expression);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public static async Task CreateUserAsync(AppUser user, string password)
    {
        using var scope = _scopeFactory.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        
        await userManager.CreateAsync(user, password);
    }
    
    public static async Task CreateRoleAsync(string roleName)
    {
        using var scope = _scopeFactory.CreateScope();
        
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
        
        var userRole = new AppRole
        {
            Name = roleName
        };

        await roleManager.CreateAsync(userRole);
    }

    public static async Task<string> GenerateEmailConfirmationTokenAsync(AppUser appUser)
    {
        using var scope = _scopeFactory.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        return await userManager.GenerateEmailConfirmationTokenAsync(appUser);
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}