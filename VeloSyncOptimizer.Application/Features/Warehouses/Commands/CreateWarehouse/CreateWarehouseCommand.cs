using MediatR;
using VeloSyncOptimizer.Application.Features.Warehouses.DTOs;

namespace VeloSyncOptimizer.Application.Features.Warehouses.Commands.CreateWarehouse;

/// <summary>
/// Command = write operation — creates a new warehouse.
/// Returns the newly generated Warehouse Id.
/// </summary>
public record CreateWarehouseCommand(
    string   Code,
    string   Name,
    string?  AddressLine1,
    string?  City,
    string?  State,
    string?  Country,
    string?  PostalCode,
    decimal? Latitude,
    decimal? Longitude,
    int      TotalCapacity,
    int?    ManagerId
) : IRequest<int>;
