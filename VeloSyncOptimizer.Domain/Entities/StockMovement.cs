using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSyncOptimizer.Domain.Entities;

[Table("StockMovements", Schema = "inventory")]
[Index("ProductId", "WarehouseId", "CreatedAt", Name = "IX_StockMov_Velocity")]
[Index("WarehouseId", "CreatedAt", Name = "IX_StockMov_Warehouse_Date", IsDescending = new[] { false, true })]
public partial class StockMovement
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int WarehouseId { get; set; }

    public byte MovementTypeId { get; set; }

    public int Quantity { get; set; }

    public int QuantityBefore { get; set; }

    public int QuantityAfter { get; set; }

    [StringLength(100)]
    public string? Reference { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("CreatedByUserId")]
    [InverseProperty("StockMovements")]
    public virtual User? CreatedByUser { get; set; }

    [ForeignKey("MovementTypeId")]
    [InverseProperty("StockMovements")]
    public virtual MovementType MovementType { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("StockMovements")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("WarehouseId")]
    [InverseProperty("StockMovements")]
    public virtual Warehouse Warehouse { get; set; } = null!;

    [InverseProperty("DestMovement")]
    public virtual ICollection<WarehouseTransfer> WarehouseTransferDestMovements { get; set; } = new List<WarehouseTransfer>();

    [InverseProperty("SourceMovement")]
    public virtual ICollection<WarehouseTransfer> WarehouseTransferSourceMovements { get; set; } = new List<WarehouseTransfer>();
}
