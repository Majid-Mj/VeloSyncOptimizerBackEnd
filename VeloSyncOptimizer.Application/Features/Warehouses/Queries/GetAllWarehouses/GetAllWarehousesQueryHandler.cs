using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces;
using VeloSyncOptimizer.Application.Features.Warehouses.DTOs;

namespace VeloSyncOptimizer.Application.Features.Warehouses.Queries.GetAllWarehouses;

public class GetAllWarehousesQueryHandler
    : IRequestHandler<GetAllWarehousesQuery, List<WarehouseDto>>
{
    // ✅ Only interface — no Dapper, no AppDbContext here
    private readonly IWarehouseRepository _warehouseRepo;

    public GetAllWarehousesQueryHandler(IWarehouseRepository warehouseRepo)
    {
        _warehouseRepo = warehouseRepo;
    }

    public async Task<List<WarehouseDto>> Handle(
        GetAllWarehousesQuery request,
        CancellationToken cancellationToken)
    {
        var warehouses = await _warehouseRepo.GetAllAsync(cancellationToken);
        return warehouses.ToList();
    }
}
