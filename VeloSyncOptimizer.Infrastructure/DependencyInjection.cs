using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VeloSyncOptimizer.Application.Common.Interfaces;
using VeloSyncOptimizer.Infrastructure.Persistence.Context;
using VeloSyncOptimizer.Infrastructure.Persistence.Repositories;
using VeloSyncOptimizer.Infrastructure.Persistence.Services;

namespace VeloSyncOptimizer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // 🔹 DbContext (EF Core for writes)
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        // 🔹 Map interface → DbContext abstraction
        // services.AddScoped<IAppDbContext>(sp =>
        //    sp.GetRequiredService<AppDbContext>());

        // 🔹 Dapper connection
        // services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();

        // 🔹 Query (Read)
        services.AddScoped<IUserRepository, UserRepository>();
        
        // Services
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordService, PasswordService>();

        return services;
    }
}
