using Dapper;
using global::VeloSyncOptimizer.Application.Common.Interfaces;
using global::VeloSyncOptimizer.Infrastructure.Persistence.Context;
using global::VeloSyncOptimizer.Infrastructure.Persistence.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;
using VeloSyncOptimizer.Domain.Entities;

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

        using var conn = new SqlConnection(_db.Database.GetConnectionString());
        return await conn.QueryFirstOrDefaultAsync<User>(
            "identity.sp_GetUserByEmail",
             new { Email = email },
              commandType: CommandType.StoredProcedure
              );
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
    {
        using var conn = new SqlConnection(_db.Database.GetConnectionString());
        return await conn.ExecuteScalarAsync<bool>("identity.sp_UserExistsByEmail", new { Email = email },
        commandType: CommandType.StoredProcedure
        );
    }

    public async Task<Guid> CreateAsync(User user, CancellationToken ct)
    {
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        _db.Users.Add(user);

        await _db.SaveChangesAsync(ct);

        return user.Id;
    }

    public async Task SaveRefreshTokenAsync(
        RefreshToken token,
        CancellationToken ct)
    {
        token.CreatedAt = DateTime.UtcNow;
        token.IsRevoked = false;

        await _db.RefreshTokens.AddAsync(token, ct); 
        await _db.SaveChangesAsync(ct);
    }

}
