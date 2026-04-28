using VeloSyncOptimizer.Application.Features.Warehouses.Commands.CreateWarehouse;
using VeloSyncOptimizer.Application.Features.Warehouses.DTOs;

namespace VeloSyncOptimizer.Application.Common.Interfaces;

public interface IWarehouseRepository
{
    // ── Queries (Dapper → Stored Procedure) ──────────────────────────────
    Task<IEnumerable<WarehouseDto>> GetAllAsync(CancellationToken ct);

    // ── Commands (EF Core) ────────────────────────────────────────────────
    Task<Guid> CreateAsync(CreateWarehouseCommand command, CancellationToken ct);
}
