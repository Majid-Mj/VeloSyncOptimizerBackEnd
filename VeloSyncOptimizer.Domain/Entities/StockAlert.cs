using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSyncOptimizer.Domain.Entities;

[Table("StockAlerts", Schema = "inventory")]
[Index("ProductId", "CreatedAt", Name = "IX_Alerts_Product", IsDescending = new[] { false, true })]
[Index("WarehouseId", "IsRead", "CreatedAt", Name = "IX_Alerts_Warehouse", IsDescending = new[] { false, false, true })]
public partial class StockAlert
{
    [Key]
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid WarehouseId { get; set; }

    public byte SeverityId { get; set; }

    [StringLength(50)]
    public string AlertType { get; set; } = null!;

    [StringLength(500)]
    public string Message { get; set; } = null!;

    public bool IsRead { get; set; }

    public DateTime? ReadAt { get; set; }

    public Guid? ReadByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("StockAlerts")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("ReadByUserId")]
    [InverseProperty("StockAlerts")]
    public virtual User? ReadByUser { get; set; }

    [ForeignKey("SeverityId")]
    [InverseProperty("StockAlerts")]
    public virtual AlertSeverity Severity { get; set; } = null!;

    [ForeignKey("WarehouseId")]
    [InverseProperty("StockAlerts")]
    public virtual Warehouse Warehouse { get; set; } = null!;
}
