namespace VeloSyncOptimizer.Application.Common.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(Guid id, CancellationToken ct);
    Task<List<T>> GetAllAsync(CancellationToken ct);


    Task AddAsync(T entity, CancellationToken ct);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct);


    void Update(T entity);
    void Remove(T entity);

    Task<int> SaveChangesAsync(CancellationToken ct);


}


