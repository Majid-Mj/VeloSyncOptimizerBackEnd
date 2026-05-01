using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;
using VeloSyncOptimizer.Application.Common.Interfaces.Services;
using VeloSyncOptimizer.Domain.Entities;

public class CreateCategoryHandler
    : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly IGenericRepository<Category> _repo;
    private readonly ICategoryRepository _queryRepo;
    private readonly ICacheService _cache;

    public CreateCategoryHandler(
        IGenericRepository<Category> repo,
        ICategoryRepository queryRepo,
        ICacheService cache)
    {
        _repo = repo;
        _queryRepo = queryRepo;
        _cache = cache;
    }

    public async Task<int> Handle(
        CreateCategoryCommand request,
        CancellationToken ct)
    {
        // Validate name
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new Exception("Category name is required");

        // Validate ParentId exists (if provided)
        if (request.ParentId.HasValue)
        {
            var parent = await _repo.GetByIdAsync(request.ParentId.Value, ct);

            if (parent == null)
                throw new Exception("Parent category not found");
        }

        //  Create entity
        var category = new Category
        {
            Name = request.Name.Trim(),
            ParentId = request.ParentId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        // Save
        await _repo.AddAsync(category, ct);
        await _repo.SaveChangesAsync(ct);

        //Invalidate cache
        await _cache.RemoveAsync("categories:all");

        return category.Id;
    }
}