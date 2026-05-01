using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSyncOptimizer.Domain.Entities;

[Table("WarehouseTransfers", Schema = "inventory")]
[Index("DestWarehouseId", Name = "IX_Transfers_Dest")]
[Index("ProductId", "CreatedAt", Name = "IX_Transfers_Product", IsDescending = new[] { false, true })]
[Index("SourceWarehouseId", Name = "IX_Transfers_Source")]
[Index("TransferNumber", Name = "UQ_Transfers_Number", IsUnique = true)]
public partial class WarehouseTransfer
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    public string TransferNumber { get; set; } = null!;

    public int ProductId { get; set; }

    public int SourceWarehouseId { get; set; }

    public int DestWarehouseId { get; set; }

    public int Quantity { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [StringLength(500)]
    public string? Notes { get; set; }

    public int? InitiatedByUserId { get; set; }

    public DateTime? CompletedAt { get; set; }

    public int? SourceMovementId { get; set; }

    public int? DestMovementId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [ForeignKey("DestMovementId")]
    [InverseProperty("WarehouseTransferDestMovements")]
    public virtual StockMovement? DestMovement { get; set; }

    [ForeignKey("DestWarehouseId")]
    [InverseProperty("WarehouseTransferDestWarehouses")]
    public virtual Warehouse DestWarehouse { get; set; } = null!;

    [ForeignKey("InitiatedByUserId")]
    [InverseProperty("WarehouseTransfers")]
    public virtual User? InitiatedByUser { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("WarehouseTransfers")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("SourceMovementId")]
    [InverseProperty("WarehouseTransferSourceMovements")]
    public virtual StockMovement? SourceMovement { get; set; }

    [ForeignKey("SourceWarehouseId")]
    [InverseProperty("WarehouseTransferSourceWarehouses")]
    public virtual Warehouse SourceWarehouse { get; set; } = null!;
}
