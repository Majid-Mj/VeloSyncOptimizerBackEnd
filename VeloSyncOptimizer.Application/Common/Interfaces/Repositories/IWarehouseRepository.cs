using VeloSyncOptimizer.Application.Features.Warehouses.Commands.CreateWarehouse;
using VeloSyncOptimizer.Application.Features.Warehouses.DTOs;

namespace VeloSyncOptimizer.Application.Common.Interfaces.Repositories;

public interface IWarehouseRepository
{
    // ── Queries (Dapper → Stored Procedure) ──────────────────────────────
    Task<IEnumerable<WarehouseDto>> GetAllAsync(CancellationToken ct);

    // ── Commands (EF Core) ────────────────────────────────────────────────
    Task<Guid> CreateAsync(CreateWarehouseCommand command, CancellationToken ct);


    Task<WarehouseDto?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<bool> UpdateAsync(WarehouseDto warehouse, CancellationToken ct);

    Task<bool> SoftDeleteAsync(Guid id, CancellationToken ct);
}
