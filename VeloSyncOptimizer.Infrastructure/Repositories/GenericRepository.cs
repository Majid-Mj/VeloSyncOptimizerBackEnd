using Microsoft.EntityFrameworkCore;
using VeloSyncOptimizer.Infrastructure.Persistence.Context;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _db;

    public GenericRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _db.Set<T>().FindAsync(new object[] { id }, ct);
    }

    public async Task AddAsync(T entity, CancellationToken ct)
    {
        await _db.Set<T>().AddAsync(entity, ct);
    }

    public void Update(T entity)
    {
        _db.Set<T>().Update(entity);
    }

    public void Remove(T entity)
    {
        _db.Set<T>().Remove(entity);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return _db.SaveChangesAsync(ct);
    }
}