using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;
using VeloSyncOptimizer.Application.Common.Interfaces.Services;
using VeloSyncOptimizer.Domain.Entities;

public class CreateSupplierHandler
    : IRequestHandler<CreateSupplierCommand, Guid>
{
    private readonly IGenericRepository<Supplier> _repo;
    private readonly ICategoryRepository _queryRepo; // Dapper (for duplicate check)
    private readonly ICacheService _cache;

    public CreateSupplierHandler(
        IGenericRepository<Supplier> repo,
        ICategoryRepository queryRepo,
        ICacheService cache)
    {
        _repo = repo;
        _queryRepo = queryRepo;
        _cache = cache;
    }

    public async Task<Guid> Handle(CreateSupplierCommand request, CancellationToken ct)
    {
        // Duplicate check (by Name)
        var existing = (await _queryRepo.QueryAsync<Supplier>(
            "inventory.sp_CheckSupplierExists",
            new { request.Name }))
            .FirstOrDefault();

        if (existing != null)
            throw new Exception("Supplier with this name already exists");

        // Create entity
        var supplier = new Supplier
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            ContactEmail = request.ContactEmail,
            ContactPhone = request.ContactPhone,
            CreatedAt = DateTime.UtcNow,
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