using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;
using VeloSyncOptimizer.Application.Common.Interfaces.Services;
using VeloSyncOptimizer.Domain.Entities;

public class DeleteSupplierHandler
    : IRequestHandler<DeleteSupplierCommand, bool>
{
    private readonly IGenericRepository<Supplier> _repo;
    private readonly ICacheService _cache;

    public DeleteSupplierHandler(
        IGenericRepository<Supplier> repo,
        ICacheService cache)
    {
        _repo = repo;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteSupplierCommand request, CancellationToken ct)
    {
        //Get supplier
        var supplier = await _repo.GetByIdAsync(request.Id, ct);

        if (supplier == null)
            throw new Exception("Supplier not found");

        if (!supplier.IsActive)
            throw new Exception("Supplier is already inactive");

        //Soft delete (Deactivate)
        supplier.IsActive = false;
        supplier.UpdatedAt = DateTime.UtcNow;

        _repo.Update(supplier);
        await _repo.SaveChangesAsync(ct);

        //Cache invalidation
        await _cache.RemoveAsync("suppliers:all");
        await _cache.RemoveAsync($"supplier:{request.Id}");

        return true;
    }
}