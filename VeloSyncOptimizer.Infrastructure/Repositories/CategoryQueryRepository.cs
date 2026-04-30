using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using VeloSyncOptimizer.Infrastructure.Persistence.Context;

public class CategoryQueryRepository : ICategoryQueryRepository
{
    private readonly AppDbContext _context;

    public CategoryQueryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        object? parameters = null,
        CommandType commandType = CommandType.StoredProcedure)
    {
        var connection = _context.Database.GetDbConnection();

        if (connection.State == ConnectionState.Closed)
            await connection.OpenAsync();

        return await connection.QueryAsync<T>(
            sql,
            parameters,
            commandType: commandType);
    }
}