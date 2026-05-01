using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;

public class GetProductsHandler
    : IRequestHandler<GetProductsQuery, PagedResult<ProductResponseDto>>
{
    private readonly IDapperRepository _repo;

    public GetProductsHandler(IDapperRepository repo)
    {
        _repo = repo;
    }

    public async Task<PagedResult<ProductResponseDto>> Handle(
        GetProductsQuery request,
        CancellationToken ct)
    {
        var (items, total) = await _repo.QueryPagedAsync<ProductResponseDto>(
            "inventory.sp_GetProductsPaged",
            new { request.PageNumber, request.PageSize },
            ct);

        return new PagedResult<ProductResponseDto>
        {
            Items = items.ToList(),
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = total
        };
    }
}