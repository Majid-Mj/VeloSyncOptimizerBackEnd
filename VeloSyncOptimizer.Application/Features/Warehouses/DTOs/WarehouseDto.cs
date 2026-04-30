namespace VeloSyncOptimizer.Application.Features.Warehouses.DTOs;

public class WarehouseDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }

    public int TotalCapacity { get; set; }
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}


