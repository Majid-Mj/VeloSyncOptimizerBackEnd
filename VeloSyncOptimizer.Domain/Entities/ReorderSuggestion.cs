using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSyncOptimizer.Domain.Entities;

[Table("ReorderSuggestions", Schema = "inventory")]
[Index("SeverityId", "IsActioned", Name = "IX_Reorder_Severity")]
[Index("WarehouseId", "IsActioned", "GeneratedAt", Name = "IX_Reorder_Warehouse", IsDescending = new[] { false, false, true })]
public partial class ReorderSuggestion
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int WarehouseId { get; set; }

    public byte SeverityId { get; set; }

    public int CurrentStock { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal DaysOfStockLeft { get; set; }

    public int SuggestedOrderQty { get; set; }

    [Column(TypeName = "decimal(10, 4)")]
    public decimal RollingAvgDaily { get; set; }

    [Column(TypeName = "decimal(10, 4)")]
    public decimal RiskScore { get; set; }

    public bool IsActioned { get; set; }

    public DateTime? ActionedAt { get; set; }

    public int? PurchaseOrderId { get; set; }

    public DateTime GeneratedAt { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ReorderSuggestions")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("PurchaseOrderId")]
    [InverseProperty("ReorderSuggestions")]
    public virtual PurchaseOrder? PurchaseOrder { get; set; }

    [ForeignKey("SeverityId")]
    [InverseProperty("ReorderSuggestions")]
    public virtual AlertSeverity Severity { get; set; } = null!;

    [ForeignKey("WarehouseId")]
    [InverseProperty("ReorderSuggestions")]
    public virtual Warehouse Warehouse { get; set; } = null!;
}
