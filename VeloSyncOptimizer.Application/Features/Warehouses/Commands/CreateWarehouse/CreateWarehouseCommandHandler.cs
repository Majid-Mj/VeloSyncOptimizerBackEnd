using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces;
using VeloSyncOptimizer.Application.Features.Warehouses.Commands.CreateWarehouse;

namespace VeloSyncOptimizer.Application.Features.Warehouses.Commands.CreateWarehouse;

public class CreateWarehouseCommandHandler
    : IRequestHandler<CreateWarehouseCommand, Guid>
{
    // ✅ Only the interface — no EF Core, no DbContext here
    private readonly IWarehouseRepository _warehouseRepo;

    public CreateWarehouseCommandHandler(IWarehouseRepository warehouseRepo)
    {
        _warehouseRepo = warehouseRepo;
    }

    public async Task<Guid> Handle(
        CreateWarehouseCommand request,
        CancellationToken cancellationToken)
    {
        return await _warehouseRepo.CreateAsync(request, cancellationToken);
    }
}
