using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSyncOptimizer.Domain.Entities;

[Table("Warehouses", Schema = "inventory")]
[Index("Code", Name = "UQ_Warehouses_Code", IsUnique = true)]
public partial class Warehouse
{
    [Key]
    public int Id { get; set; }

    [StringLength(20)]
    public string Code { get; set; } = null!;

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(300)]
    public string? AddressLine1 { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? State { get; set; }

    [StringLength(100)]
    public string? Country { get; set; }

    [StringLength(20)]
    public string? PostalCode { get; set; }

    [Column(TypeName = "decimal(9, 6)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(9, 6)")]
    public decimal? Longitude { get; set; }

    public int TotalCapacity { get; set; }

    public int? ManagerId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    [ForeignKey("ManagerId")]
    [InverseProperty("Warehouses")]
    public virtual User? Manager { get; set; }

    [InverseProperty("Warehouse")]
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    [InverseProperty("Warehouse")]
    public virtual ICollection<ReorderSuggestion> ReorderSuggestions { get; set; } = new List<ReorderSuggestion>();

    [InverseProperty("Warehouse")]
    public virtual ICollection<StockAlert> StockAlerts { get; set; } = new List<StockAlert>();

    [InverseProperty("Warehouse")]
    public virtual ICollection<StockLevel> StockLevels { get; set; } = new List<StockLevel>();

    [InverseProperty("Warehouse")]
    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

    [InverseProperty("Warehouse")]
    public virtual ICollection<VelocityCache> VelocityCaches { get; set; } = new List<VelocityCache>();

    [InverseProperty("DestWarehouse")]
    public virtual ICollection<WarehouseTransfer> WarehouseTransferDestWarehouses { get; set; } = new List<WarehouseTransfer>();

    [InverseProperty("SourceWarehouse")]
    public virtual ICollection<WarehouseTransfer> WarehouseTransferSourceWarehouses { get; set; } = new List<WarehouseTransfer>();
}
