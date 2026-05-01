using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSyncOptimizer.Domain.Entities;

[Table("PurchaseOrders", Schema = "procurement")]
[Index("StatusId", "CreatedAt", Name = "IX_PO_StatusId", IsDescending = new[] { false, true })]
[Index("SupplierId", "StatusId", Name = "IX_PO_SupplierId")]
[Index("WarehouseId", "StatusId", Name = "IX_PO_WarehouseId")]
[Index("PONumber", Name = "UQ_PO_Number", IsUnique = true)]
public partial class PurchaseOrder
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    public string PONumber { get; set; } = null!;

    public int SupplierId { get; set; }

    public int WarehouseId { get; set; }

    public byte StatusId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal TotalAmount { get; set; }

    public DateOnly? ExpectedDate { get; set; }

    public int? ApprovedByUserId { get; set; }

    public DateTime? ApprovedAt { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("ApprovedByUserId")]
    [InverseProperty("PurchaseOrderApprovedByUsers")]
    public virtual User? ApprovedByUser { get; set; }

    [ForeignKey("CreatedByUserId")]
    [InverseProperty("PurchaseOrderCreatedByUsers")]
    public virtual User? CreatedByUser { get; set; }

    [InverseProperty("PurchaseOrder")]
    public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; } = new List<PurchaseOrderLine>();

    [InverseProperty("PurchaseOrder")]
    public virtual ICollection<ReorderSuggestion> ReorderSuggestions { get; set; } = new List<ReorderSuggestion>();

    [ForeignKey("StatusId")]
    [InverseProperty("PurchaseOrders")]
    public virtual PurchaseOrderStatus Status { get; set; } = null!;

    [ForeignKey("SupplierId")]
    [InverseProperty("PurchaseOrders")]
    public virtual Supplier Supplier { get; set; } = null!;

    [InverseProperty("PurchaseOrder")]
    public virtual ICollection<SupplierDelivery> SupplierDeliveries { get; set; } = new List<SupplierDelivery>();

    [ForeignKey("WarehouseId")]
    [InverseProperty("PurchaseOrders")]
    public virtual Warehouse Warehouse { get; set; } = null!;
}
