using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VeloSyncOptimizer.Application.Common.Interfaces;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;
using VeloSyncOptimizer.Infrastructure.Persistence.Context;
using VeloSyncOptimizer.Infrastructure.Persistence.Repositories;
using VeloSyncOptimizer.Infrastructure.Persistence.Services;
using VeloSyncOptimizer.Infrastructure.Repositories;

namespace VeloSyncOptimizer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // 🔹 DbContext (EF Core for writes)
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        // 🔹 Query (Read)
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        
        // Services
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordService, PasswordService>();

        //GenericRepository injection
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        return services;
    }
}
