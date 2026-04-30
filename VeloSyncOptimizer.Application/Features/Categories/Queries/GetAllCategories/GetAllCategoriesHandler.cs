using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces.Services;

public class GetAllCategoriesHandler
    : IRequestHandler<GetAllCategoriesQuery, List<CategoryResponseDto>>
{
    private readonly ICategoryRepository _repo;
    private readonly ICacheService _cache;

    public GetAllCategoriesHandler(
        ICategoryRepository repo,
        ICacheService cache)
    {
        _repo = repo;
        _cache = cache;
    }

    public async Task<List<CategoryResponseDto>> Handle(
        GetAllCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        const string cacheKey = "categories:all";

        // Try cache
        var cached = await _cache.GetAsync<List<CategoryResponseDto>>(cacheKey);
        if (cached != null)
            return cached;

        // Fetch from DB
        var categories = (await _repo.QueryAsync<CategoryResponseDto>(
            "inventory.sp_GetAllCategories"))
            .ToList();

        // 
        var lookup = categories.ToDictionary(x => x.Id);
        var roots = new List<CategoryResponseDto>();

        foreach (var cat in categories)
        {
            if (cat.ParentId.HasValue && lookup.ContainsKey(cat.ParentId.Value))
                lookup[cat.ParentId.Value].Children.Add(cat);
            else
                roots.Add(cat);
        }

        // Cache result (10 min)
        await _cache.SetAsync(cacheKey, roots, TimeSpan.FromMinutes(10));

        return roots;
    }
}