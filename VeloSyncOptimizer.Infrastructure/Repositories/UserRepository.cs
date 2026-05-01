using Dapper;
using VeloSyncOptimizer.Application.Common.Interfaces;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;
using VeloSyncOptimizer.Application.Features.Users.DTOs;
using VeloSyncOptimizer.Domain.Entities;
using VeloSyncOptimizer.Infrastructure.Dapper.Queries;
using VeloSyncOptimizer.Infrastructure.Persistence.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
        return await conn.ExecuteScalarAsync<bool>(
            "identity.sp_UserExistsByEmail", 
            new { Email = email },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<int> CreateAsync(User user, CancellationToken ct)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);
        return user.Id;
    }

    public async Task SaveRefreshTokenAsync(RefreshToken token, CancellationToken ct)
    {
        await _db.RefreshTokens.AddAsync(token, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _db.Users.FindAsync(id);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<UserDto>> GetPendingUsersAsync(CancellationToken ct)
    {
        using var conn = new SqlConnection(_db.Database.GetConnectionString());

        var result = await conn.QueryAsync<UserDto>(
            UserQueries.GetPendingUsers,
            commandType: CommandType.StoredProcedure
            );  

        return result.ToList();
    }


    public async Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken ct)
    {
        using var conn = new SqlConnection(_db.Database.GetConnectionString());

        await conn.ExecuteAsync(
            "identity.sp_RevokeRefreshToken",
            new { Token = refreshToken },
            commandType: CommandType.StoredProcedure
        );
    }



    public async Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken ct)
    {
        using var conn = new SqlConnection(_db.Database.GetConnectionString());

        return await conn.QueryFirstOrDefaultAsync<RefreshToken>(
            "identity.sp_GetRefreshToken",
            new { Token = token },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task UpdateRefreshTokenAsync(RefreshToken token, CancellationToken ct)
    {
        _db.RefreshTokens.Update(token);
        await _db.SaveChangesAsync(ct);
    }


}

