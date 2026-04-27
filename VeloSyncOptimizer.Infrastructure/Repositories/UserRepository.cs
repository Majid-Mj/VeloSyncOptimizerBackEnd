using Dapper;
using global::VeloSyncOptimizer.Application.Common.Interfaces;
using global::VeloSyncOptimizer.Infrastructure.Persistence.Context;
using global::VeloSyncOptimizer.Infrastructure.Persistence.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace VeloSyncOptimizer.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        var sql = @"
            SELECT 
                u.Id,
                u.Email,
                u.PasswordHash,
                u.FirstName,
                u.LastName,
                u.RoleId,
                u.IsActive,
                r.Name AS RoleName
            FROM [identity].Users u
            JOIN [identity].UserRoles r ON u.RoleId = r.Id
            WHERE u.Email     = @Email
              AND u.IsDeleted = 0
              AND u.IsActive  = 1";

        using var conn = new SqlConnection(_db.Database.GetConnectionString());
        return await conn.QueryFirstOrDefaultAsync<User>(
            sql, new { Email = email });
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
    {
        var sql = "SELECT CAST(CASE WHEN COUNT(1) > 0 THEN 1 ELSE 0 END AS BIT) FROM [identity].Users WHERE Email = @Email AND IsDeleted = 0";
        using var conn = new SqlConnection(_db.Database.GetConnectionString());
        return await conn.ExecuteScalarAsync<bool>(sql, new { Email = email });
    }

    public async Task<Guid> CreateAsync(User user, CancellationToken ct)
    {
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        var sql = @"
            INSERT INTO [identity].Users (Id, Email, PasswordHash, FirstName, LastName, RoleId, IsActive, CreatedAt, UpdatedAt, IsDeleted)
            VALUES (@Id, @Email, @PasswordHash, @FirstName, @LastName, @RoleId, @IsActive, @CreatedAt, @UpdatedAt, 0)";
        using var conn = new SqlConnection(_db.Database.GetConnectionString());
        await conn.ExecuteAsync(sql, user);
        return user.Id;
    }
    public async Task SaveRefreshTokenAsync(VeloSyncOptimizer.Domain.Entities.RefreshToken token, CancellationToken ct)
    {
        var sql = @"
            INSERT INTO [identity].RefreshTokens (Id, UserId, Token, ExpiresAt, IsRevoked, CreatedAt)
            VALUES (@Id, @UserId, @Token, @ExpiresAt, @IsRevoked, @CreatedAt)";
        using var conn = new SqlConnection(_db.Database.GetConnectionString());
        await conn.ExecuteAsync(sql, token);
    }
}
