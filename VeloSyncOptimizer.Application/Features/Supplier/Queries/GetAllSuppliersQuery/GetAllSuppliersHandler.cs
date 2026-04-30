using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces.Services;

public class GetAllSuppliersHandler
    : IRequestHandler<GetAllSuppliersQuery, List<SupplierResponseDto>>
{
    private readonly ICategoryRepository _repo; 
    private readonly ICacheService _cache;

    public GetAllSuppliersHandler(
        ICategoryRepository repo,
        ICacheService cache)
    {
        _repo = repo;
        _cache = cache;
    }

    public async Task<List<SupplierResponseDto>> Handle(
        GetAllSuppliersQuery request,
        CancellationToken ct)
    {
        const string cacheKey = "suppliers:all";

        // Try cache
        var cached = await _cache.GetAsync<List<SupplierResponseDto>>(cacheKey);
        if (cached != null)
            return cached;

        //Fetch from DB
        var suppliers = (await _repo.QueryAsync<SupplierResponseDto>(
            "inventory.sp_GetAllSuppliers"))
            .ToList();

        await _cache.SetAsync(cacheKey, suppliers, TimeSpan.FromMinutes(10));

        return suppliers;
    }
}