using System.Data;

public interface IDapperRepository
{
    Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        object? parameters = null,
        CommandType commandType = CommandType.StoredProcedure,
        CancellationToken ct = default);

    Task<(IEnumerable<T>, int)> QueryPagedAsync<T>(
        string sql,
        object? parameters = null,
        CancellationToken ct = default);
}