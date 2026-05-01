using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using VeloSyncOptimizer.Infrastructure.Persistence.Context;

public class DapperRepository : IDapperRepository
{
    private readonly AppDbContext _db;

    public DapperRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(
     string sql,
     object? parameters = null,
     CommandType commandType = CommandType.StoredProcedure,
     CancellationToken ct = default)
    {
        var connection = _db.Database.GetDbConnection();

        if (connection.State == ConnectionState.Closed)
            await connection.OpenAsync(ct);

        return await connection.QueryAsync<T>(
            new CommandDefinition(sql, parameters, commandType: commandType, cancellationToken: ct));
    }

    public async Task<(IEnumerable<T>, int)> QueryPagedAsync<T>(
        string sql,
        object? parameters = null,
        CancellationToken ct = default)
    {
        var connection = _db.Database.GetDbConnection();

        if (connection.State == ConnectionState.Closed)
            await connection.OpenAsync(ct);

        using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(
                sql,
                parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct));

        var items = await multi.ReadAsync<T>();
        var total = await multi.ReadFirstAsync<int>();

        return (items, total);
    }
}