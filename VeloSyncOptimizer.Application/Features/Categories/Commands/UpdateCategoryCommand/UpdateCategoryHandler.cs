using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;
using VeloSyncOptimizer.Application.Common.Interfaces.Services;
using VeloSyncOptimizer.Domain.Entities;

public class UpdateCategoryHandler
    : IRequestHandler<UpdateCategoryCommand, bool>
{
    private readonly IGenericRepository<Category> _repo;
    private readonly ICategoryRepository _queryRepo;
    private readonly ICacheService _cache;

    public UpdateCategoryHandler(
        IGenericRepository<Category> repo,
        ICategoryRepository queryRepo,
        ICacheService cache)
    {
        _repo = repo;
        _queryRepo = queryRepo;
        _cache = cache;
    }

    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken ct)
    {
        // Get existing category
        var category = await _repo.GetByIdAsync(request.Id, ct);

        if (category == null || category.IsDeleted)
            throw new Exception("Category not found");

        // Prevent self-parent
        if (request.ParentId == request.Id)
            throw new Exception("Category cannot be its own parent");

        // Validate parent exists
        if (request.ParentId.HasValue)
        {
            var parent = await _repo.GetByIdAsync(request.ParentId.Value, ct);

            if (parent == null || parent.IsDeleted)
                throw new Exception("Parent category not found");
        }

        // Prevent circular dependency
        if (request.ParentId.HasValue)
        {
            var allCategories = (await _queryRepo
                .QueryAsync<Category>("inventory.sp_GetAllCategoriesFlat"))
                .ToList();

            var lookup = allCategories.ToDictionary(x => x.Id);

            var currentParentId = request.ParentId;

            while (currentParentId != null)
            {
                if (currentParentId == request.Id)
                    throw new Exception("Circular dependency detected");

                currentParentId = lookup.ContainsKey(currentParentId.Value)
                    ? lookup[currentParentId.Value].ParentId
                    : null;
            }
        }

        // Update fields
        if (!string.IsNullOrWhiteSpace(request.Name))
            category.Name = request.Name.Trim();

        if (request.ParentId != null)
            category.ParentId = request.ParentId;

        category.UpdatedAt = DateTime.UtcNow;

        // Save
        _repo.Update(category);
        await _repo.SaveChangesAsync(ct);

        // Invalidate cache
        await _cache.RemoveAsync("categories:all");

        return true;
    }
}