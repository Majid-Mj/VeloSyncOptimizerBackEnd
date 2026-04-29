using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSyncOptimizer.Domain.Entities;

[Table("VelocityCache", Schema = "inventory")]
[Index("ComputedAt", Name = "IX_Velocity_ComputedAt", AllDescending = true)]
[Index("ProductId", "WarehouseId", Name = "UQ_VelocityCache_Prod_WH", IsUnique = true)]
public partial class VelocityCache
{
    [Key]
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid WarehouseId { get; set; }

    [Column(TypeName = "decimal(10, 4)")]
    public decimal AvgDaily30 { get; set; }

    [Column(TypeName = "decimal(10, 4)")]
    public decimal AvgDaily60 { get; set; }

    [Column(TypeName = "decimal(10, 4)")]
    public decimal AvgDaily90 { get; set; }

    public int PeakDailyQty { get; set; }

    public DateTime ComputedAt { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("VelocityCaches")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("WarehouseId")]
    [InverseProperty("VelocityCaches")]
    public virtual Warehouse Warehouse { get; set; } = null!;
}
