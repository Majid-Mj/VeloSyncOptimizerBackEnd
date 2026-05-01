using VeloSyncOptimizer.Application.Features.Warehouses.Commands.CreateWarehouse;
using VeloSyncOptimizer.Application.Features.Warehouses.DTOs;

namespace VeloSyncOptimizer.Application.Common.Interfaces.Repositories;

public interface IWarehouseRepository
{
    // ── Queries (Dapper → Stored Procedure) ──────────────────────────────
    Task<IEnumerable<WarehouseDto>> GetAllAsync(CancellationToken ct);

    // ── Commands (EF Core) ────────────────────────────────────────────────
    Task<int> CreateAsync(CreateWarehouseCommand command, CancellationToken ct);


    Task<WarehouseDto?> GetByIdAsync(int id, CancellationToken ct);

    Task<bool> UpdateAsync(WarehouseDto warehouse, CancellationToken ct);

    Task<bool> SoftDeleteAsync(int id, CancellationToken ct);
}
