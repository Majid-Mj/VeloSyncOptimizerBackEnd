using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSyncOptimizer.Infrastructure.Persistence.Models;

[Table("Products", Schema = "inventory")]
[Index("SKU", Name = "UQ_Products_SKU", IsUnique = true)]
public partial class Product
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(100)]
    public string SKU { get; set; } = null!;

    [StringLength(300)]
    public string Name { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    public Guid? CategoryId { get; set; }

    public Guid? SupplierId { get; set; }

    [StringLength(30)]
    public string UnitOfMeasure { get; set; } = null!;

    [Column(TypeName = "decimal(18, 4)")]
    public decimal UnitCost { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal UnitPrice { get; set; }

    public int ReorderQty { get; set; }

    public int SafetyStockDays { get; set; }

    public int LeadTimeDays { get; set; }

    public bool IsActive { get; set; }

    public bool IsPerishable { get; set; }

    public int? ShelfLifeDays { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category? Category { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; } = new List<PurchaseOrderLine>();

    [InverseProperty("Product")]
    public virtual ICollection<ReorderSuggestion> ReorderSuggestions { get; set; } = new List<ReorderSuggestion>();

    [InverseProperty("Product")]
    public virtual ICollection<StockAlert> StockAlerts { get; set; } = new List<StockAlert>();

    [InverseProperty("Product")]
    public virtual ICollection<StockLevel> StockLevels { get; set; } = new List<StockLevel>();

    [InverseProperty("Product")]
    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

    [ForeignKey("SupplierId")]
    [InverseProperty("Products")]
    public virtual Supplier? Supplier { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<VelocityCache> VelocityCaches { get; set; } = new List<VelocityCache>();

    [InverseProperty("Product")]
    public virtual ICollection<WarehouseTransfer> WarehouseTransfers { get; set; } = new List<WarehouseTransfer>();
}
