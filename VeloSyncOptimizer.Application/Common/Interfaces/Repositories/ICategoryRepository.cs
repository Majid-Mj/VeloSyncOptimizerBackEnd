using System.Data;

public interface ICategoryRepository
{
    Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        object? parameters = null,
        CommandType commandType = CommandType.StoredProcedure);
}