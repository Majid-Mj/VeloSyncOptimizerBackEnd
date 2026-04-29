using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSyncOptimizer.Domain.Entities;

[Table("SupplierDeliveries", Schema = "procurement")]
[Index("PurchaseOrderId", Name = "IX_Deliveries_PO")]
[Index("SupplierId", "PromisedDate", Name = "IX_Deliveries_Supplier", IsDescending = new[] { false, true })]
public partial class SupplierDelivery
{
    [Key]
    public Guid Id { get; set; }

    public Guid PurchaseOrderId { get; set; }

    public Guid SupplierId { get; set; }

    public DateOnly PromisedDate { get; set; }

    public DateOnly? ActualDate { get; set; }

    public bool? IsOnTime { get; set; }

    public int? DaysLate { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [ForeignKey("PurchaseOrderId")]
    [InverseProperty("SupplierDeliveries")]
    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;

    [ForeignKey("SupplierId")]
    [InverseProperty("SupplierDeliveries")]
    public virtual Supplier Supplier { get; set; } = null!;
}
