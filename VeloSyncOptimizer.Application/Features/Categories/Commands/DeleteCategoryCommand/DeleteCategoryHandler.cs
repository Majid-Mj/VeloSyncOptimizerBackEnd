using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;
using VeloSyncOptimizer.Application.Common.Interfaces.Services;
using VeloSyncOptimizer.Domain.Entities;

public class DeleteCategoryHandler
    : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly IGenericRepository<Category> _repo;
    private readonly ICategoryRepository _queryRepo;
    private readonly ICacheService _cache;

    public DeleteCategoryHandler(
        IGenericRepository<Category> repo,
        ICategoryRepository queryRepo,
        ICacheService cache)
    {
        _repo = repo;
        _queryRepo = queryRepo;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken ct)
    {
        //Get category
        var category = await _repo.GetByIdAsync(request.Id, ct);

        if (category == null || category.IsDeleted)
            throw new Exception("Category not found");

        //Check if has children
        var allCategories = (await _queryRepo
            .QueryAsync<Category>("inventory.sp_GetAllCategoriesFlat"))
            .ToList();

        var hasChildren = allCategories.Any(x => x.ParentId == request.Id);

        if (hasChildren)
            throw new Exception("Cannot delete category with child categories");

        // Soft delete
        category.IsDeleted = true;
        category.DeletedAt = DateTime.UtcNow;
        category.UpdatedAt = DateTime.UtcNow;

        _repo.Update(category);
        await _repo.SaveChangesAsync(ct);

        // Invalidate cache
        await _cache.RemoveAsync("categories:all");

        return true;
    }
}