using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VeloSyncOptimizer.Domain.Entities.Common;

namespace VeloSyncOptimizer.Infrastructure.Persistence.Models;

[Table("Users", Schema = "identity")]
[Index("Email", Name = "UQ_Users_Email", IsUnique = true)]
public partial class User : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(256)]
    public string Email { get; set; } = null!;

    [StringLength(512)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    public string LastName { get; set; } = null!;

    public byte RoleId { get; set; }

    public bool IsActive { get; set; }
    
    [NotMapped]
    public string RoleName { get; set; } = string.Empty;

    public DateTime? LastLoginAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    [InverseProperty("ApprovedByUser")]
    public virtual ICollection<PurchaseOrder> PurchaseOrderApprovedByUsers { get; set; } = new List<PurchaseOrder>();

    [InverseProperty("CreatedByUser")]
    public virtual ICollection<PurchaseOrder> PurchaseOrderCreatedByUsers { get; set; } = new List<PurchaseOrder>();

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual UserRole Role { get; set; } = null!;

    [InverseProperty("ReadByUser")]
    public virtual ICollection<StockAlert> StockAlerts { get; set; } = new List<StockAlert>();

    [InverseProperty("CreatedByUser")]
    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

    [InverseProperty("InitiatedByUser")]
    public virtual ICollection<WarehouseTransfer> WarehouseTransfers { get; set; } = new List<WarehouseTransfer>();

    [InverseProperty("Manager")]
    public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}
