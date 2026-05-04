using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;
using VeloSyncOptimizer.Application.Common.Interfaces.Services;
using VeloSyncOptimizer.Domain.Entities;

public class CreateSupplierHandler
    : IRequestHandler<CreateSupplierCommand, int>
{
    private readonly IGenericRepository<Supplier> _repo;
    private readonly ICacheService _cache;

    public CreateSupplierHandler(
        IGenericRepository<Supplier> repo,
        ICacheService cache)
    {
        _repo = repo;
        _cache = cache;
    }

    public async Task<int> Handle(CreateSupplierCommand request, CancellationToken ct)
    {
        // Duplicate check (by Name)
        var exists = await _repo.ExistsAsync(s => s.Name == request.Name.Trim(), ct);

        if (exists)
            throw new Exception("Supplier with this name already exists");

        // Create entity
        var supplier = new Supplier
        {
            Name = request.Name.Trim(),
            ContactEmail = request.ContactEmail,
            ContactPhone = request.ContactPhone,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        // Save
        await _repo.AddAsync(supplier, ct);
        await _repo.SaveChangesAsync(ct);

        //Cache invalidation
        await _cache.RemoveAsync("suppliers:all");

        return supplier.Id;
    }
}