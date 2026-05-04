using System.Data;
using System.Linq.Expressions;

//namespace VeloSyncOptimizer.Application.Common.Interfaces.Repositories;

//public interface IGenericRepository<T> where T : class
//{
//    Task<List<T>> GetAllAsync(CancellationToken ct);


//    Task AddAsync(T entity, CancellationToken ct);
//    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct);


//    void Update(T entity);
//    void Remove(T entity);

//    Task<int> SaveChangesAsync(CancellationToken ct);



//}
public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id, CancellationToken ct);

    Task AddAsync(T entity, CancellationToken ct);

    void Update(T entity);

    void Remove(T entity);

    Task<int> SaveChangesAsync(CancellationToken ct);

    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);
}


