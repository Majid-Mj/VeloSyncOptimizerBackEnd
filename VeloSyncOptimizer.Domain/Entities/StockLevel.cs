using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSyncOptimizer.Domain.Entities;

[Table("StockLevels", Schema = "inventory")]
[Index("ProductId", Name = "IX_StockLevels_ProductId")]
[Index("WarehouseId", "ReorderPoint", "QuantityOnHand", Name = "IX_StockLevels_Reorder")]
[Index("WarehouseId", Name = "IX_StockLevels_WarehouseId")]
[Index("ProductId", "WarehouseId", Name = "UQ_StockLevels_Prod_WH", IsUnique = true)]
public partial class StockLevel
{
    [Key]
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid WarehouseId { get; set; }

    public int QuantityOnHand { get; set; }

    public int QuantityOnOrder { get; set; }

    public int QuantityReserved { get; set; }

    public int ReorderPoint { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("StockLevels")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("WarehouseId")]
    [InverseProperty("StockLevels")]
    public virtual Warehouse Warehouse { get; set; } = null!;
}
