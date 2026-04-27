using Microsoft.EntityFrameworkCore;
using VeloSyncOptimizer.Application.Common.Interfaces;
using VeloSyncOptimizer.Infrastructure.Persistence.Context;
using VeloSyncOptimizer.Infrastructure.Persistence.Models;

namespace VeloSyncOptimizer.API.Extensions;

public static class DatabaseSeeder
{
    public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        var passwordService = serviceProvider.GetRequiredService<IPasswordService>();

        // 0. Ensure RefreshTokens table exists
        await context.Database.ExecuteSqlRawAsync(@"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RefreshTokens' AND schema_id = SCHEMA_ID('identity'))
            BEGIN
                CREATE TABLE [identity].RefreshTokens (
                    Id UNIQUEIDENTIFIER PRIMARY KEY,
                    UserId UNIQUEIDENTIFIER NOT NULL,
                    Token NVARCHAR(256) NOT NULL,
                    ExpiresAt DATETIME2 NOT NULL,
                    IsRevoked BIT NOT NULL,
                    CreatedAt DATETIME2 NOT NULL,
                    CONSTRAINT FK_RefreshTokens_Users FOREIGN KEY (UserId) REFERENCES [identity].Users(Id)
                )
            END
        ");

        // 1. Seed Roles
        if (!await context.UserRoles.AnyAsync(r => r.Id == 1))
        {
            context.UserRoles.Add(new UserRole { Id = 1, Name = "Administrator" });
        }
        if (!await context.UserRoles.AnyAsync(r => r.Id == 2))
        {
            context.UserRoles.Add(new UserRole { Id = 2, Name = "WarehouseManager" });
        }
        if (!await context.UserRoles.AnyAsync(r => r.Id == 3))
        {
            context.UserRoles.Add(new UserRole { Id = 3, Name = "ProcurementOfficer" });
        }
        
        // Wait, EF Core might track the above. Let's just save.
        try {
            await context.SaveChangesAsync();
        } catch {
            // Ignore if identity insert issue or already seeded in parallel
        }

        // 2. Seed Administrator User
        var adminEmail = "admin@velosync.com";
        if (!await context.Users.AnyAsync(u => u.Email == adminEmail))
        {
            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Email = adminEmail,
                PasswordHash = passwordService.Hash("Admin123!"),
                FirstName = "System",
                LastName = "Administrator",
                RoleId = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
        }
    }
}
