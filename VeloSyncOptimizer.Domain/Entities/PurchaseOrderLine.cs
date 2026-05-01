using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSyncOptimizer.Domain.Entities;

[Table("PurchaseOrderLines", Schema = "procurement")]
[Index("PurchaseOrderId", Name = "IX_POLines_PO")]
[Index("ProductId", Name = "IX_POLines_Product")]
public partial class PurchaseOrderLine
{
    [Key]
    public int Id { get; set; }

    public int PurchaseOrderId { get; set; }

    public int ProductId { get; set; }

    public int QuantityOrdered { get; set; }

    public int QuantityReceived { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal UnitCost { get; set; }

    [Column(TypeName = "decimal(29, 4)")]
    public decimal? LineTotal { get; set; }

    public DateTime? ReceivedAt { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("PurchaseOrderLines")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("PurchaseOrderId")]
    [InverseProperty("PurchaseOrderLines")]
    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
}
