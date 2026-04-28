namespace VeloSyncOptimizer.Application.Features.Warehouses.DTOs;

public record WarehouseDto(
    Guid   Id,
    string Code,
    string Name,
    string City,
    string State,
    string Country,
    int    TotalCapacity,
    bool   IsActive,
    DateTime CreatedAt
);
