using MediatR;
using VeloSyncOptimizer.Application.Features.Warehouses.DTOs;

namespace VeloSyncOptimizer.Application.Features.Warehouses.Queries.GetAllWarehouses;

/// <summary>
/// Query = read-only — returns all warehouses via stored procedure.
/// </summary>
public record GetAllWarehousesQuery : IRequest<List<WarehouseDto>>;
