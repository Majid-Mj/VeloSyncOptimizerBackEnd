using System.ComponentModel.DataAnnotations;

namespace VeloSyncOptimizer.Application.Features.Warehouses.DTOs;

/// <summary>
/// DTO for full warehouse replacement (PUT). All fields are required.
/// </summary>
public class ReplaceWarehouseRequestDto
{
    [Required]
    public string Code { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string State { get; set; } = string.Empty;

    [Required]
    public string Country { get; set; } = string.Empty;

    [Required]
    public int TotalCapacity { get; set; }

    [Required]
    public bool IsActive { get; set; }
}
