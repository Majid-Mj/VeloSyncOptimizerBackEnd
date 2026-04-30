using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;
using VeloSyncOptimizer.Application.Common.Interfaces.Services;
using VeloSyncOptimizer.Domain.Entities;

public class UpdateSupplierHandler
    : IRequestHandler<UpdateSupplierCommand, bool>
{
    private readonly IGenericRepository<Supplier> _repo;
    private readonly ICacheService _cache;

    public UpdateSupplierHandler(
        IGenericRepository<Supplier> repo,
        ICacheService cache)
    {
        _repo = repo;
        _cache = cache;
    }

    public async Task<bool> Handle(UpdateSupplierCommand request, CancellationToken ct)
    {
        // Get supplier
        var supplier = await _repo.GetByIdAsync(request.Id, ct);

        if (supplier == null)
            throw new Exception("Supplier not found");

        //Update fields (partial update)
        if (!string.IsNullOrWhiteSpace(request.Name))
            supplier.Name = request.Name.Trim();

        if (request.ContactEmail != null)
            supplier.ContactEmail = request.ContactEmail;

        if (request.ContactPhone != null)
            supplier.ContactPhone = request.ContactPhone;

        if (request.IsActive.HasValue)
            supplier.IsActive = request.IsActive.Value;

        supplier.UpdatedAt = DateTime.UtcNow;

        // Save
        _repo.Update(supplier);
        await _repo.SaveChangesAsync(ct);

        // Cache invalidation
        await _cache.RemoveAsync("suppliers:all");
        await _cache.RemoveAsync($"supplier:{request.Id}");

        return true;
    }
}