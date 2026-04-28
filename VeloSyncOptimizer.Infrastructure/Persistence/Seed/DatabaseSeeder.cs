using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VeloSyncOptimizer.Application.Common.Interfaces;
using VeloSyncOptimizer.Infrastructure.Persistence.Context;
using VeloSyncOptimizer.Infrastructure.Persistence.Models;

namespace VeloSyncOptimizer.Infrastructure.Persistence.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();

        // OPTIONAL but recommended: apply migrations
        await context.Database.MigrateAsync();


        if (!await context.UserRoles.AnyAsync())
        {
            context.UserRoles.AddRange(
                new UserRole { Id = 1, Name = "Administrator" },
                new UserRole { Id = 2, Name = "WarehouseManager" },
                new UserRole { Id = 3, Name = "ProcurementOfficer" }
            );

            await context.SaveChangesAsync();
        }


        const string adminEmail = "admin@velosync.com";

        var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);

        if (adminUser == null)
        {
            adminUser = new User
            {
                Id = Guid.NewGuid(),
                Email = adminEmail,
                PasswordHash = passwordService.Hash("Admin@123"),
                FirstName = "System",
                LastName = "Administrator",
                RoleId = 1,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Users.Add(adminUser);
        }
        else
        {
            // Ensure Admin password and status are correct
            adminUser.PasswordHash = passwordService.Hash("Admin@123");
            adminUser.IsActive = true;
            adminUser.IsDeleted = false;
            adminUser.UpdatedAt = DateTime.UtcNow;
            context.Users.Update(adminUser);
        }

        await context.SaveChangesAsync();
    }
}