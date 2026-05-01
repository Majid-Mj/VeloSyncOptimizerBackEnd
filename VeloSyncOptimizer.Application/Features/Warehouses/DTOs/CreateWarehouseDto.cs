namespace VeloSyncOptimizer.Application.Features.Warehouses.DTOs;

public record CreateWarehouseDto(
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
);
