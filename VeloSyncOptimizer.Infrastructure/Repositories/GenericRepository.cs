using Microsoft.EntityFrameworkCore;
using VeloSyncOptimizer.Application.Common.Interfaces;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;
using VeloSyncOptimizer.Infrastructure.Persistence.Context;


namespace VeloSyncOptimizer.Infrastructure.Repositories;


public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _db;

    public GenericRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct)
      => await _db.Set<T>().FindAsync(new object[] { id }, ct);

    public async Task<List<T>> GetAllAsync(CancellationToken ct)
        => await _db.Set<T>().AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(T entity, CancellationToken ct)
        => await _db.Set<T>().AddAsync(entity, ct);

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct)
        => await _db.Set<T>().AddRangeAsync(entities, ct);

    public void Update(T entity)
        => _db.Set<T>().Update(entity);

    public void Remove(T entity)
        => _db.Set<T>().Remove(entity);

    public Task<int> SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);

}
