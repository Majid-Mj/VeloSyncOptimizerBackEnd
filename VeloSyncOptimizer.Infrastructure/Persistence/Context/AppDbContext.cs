using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using VeloSyncOptimizer.Domain.Entities;

using VeloSyncOptimizer.Domain.Entities;

namespace VeloSyncOptimizer.Infrastructure.Persistence.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    

    public virtual DbSet<AlertSeverity> AlertSeverities { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<MovementType> MovementTypes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }

    public virtual DbSet<PurchaseOrderStatus> PurchaseOrderStatuses { get; set; }

    public virtual DbSet<ReorderSuggestion> ReorderSuggestions { get; set; }

    public virtual DbSet<StockAlert> StockAlerts { get; set; }

    public virtual DbSet<StockLevel> StockLevels { get; set; }

    public virtual DbSet<StockMovement> StockMovements { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SupplierDelivery> SupplierDeliveries { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<VelocityCache> VelocityCaches { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    public virtual DbSet<WarehouseTransfer> WarehouseTransfers { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        base.OnModelCreating(modelBuilder);
        // If you use schema "identity"
        modelBuilder.Entity<User>().ToTable("Users", "identity");
        modelBuilder.Entity<UserRole>().ToTable("UserRoles", "identity");
        modelBuilder.Entity<RefreshToken>().ToTable("RefreshTokens", "identity");

        // Optional: soft-delete global filter
        modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);



        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.User).WithMany(p => p.AuditLogs).HasConstraintName("FK_Audit_User");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK_Categories_Parent");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(e => e.CategoryId, "IX_Products_CategoryId").HasFilter("([IsDeleted]=(0))");

            entity.HasIndex(e => e.SKU, "IX_Products_SKU").HasFilter("([IsDeleted]=(0))");

            entity.HasIndex(e => e.SupplierId, "IX_Products_SupplierId").HasFilter("([IsDeleted]=(0))");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LeadTimeDays).HasDefaultValue(7);
            entity.Property(e => e.SafetyStockDays).HasDefaultValue(7);
            entity.Property(e => e.UnitOfMeasure).HasDefaultValue("Unit");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasConstraintName("FK_Products_Category");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products).HasConstraintName("FK_Products_Supplier");
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.StatusId).HasDefaultValue((byte)1);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.ApprovedByUser).WithMany(p => p.PurchaseOrderApprovedByUsers).HasConstraintName("FK_PO_ApprovedBy");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.PurchaseOrderCreatedByUsers).HasConstraintName("FK_PO_CreatedBy");

            entity.HasOne(d => d.Status).WithMany(p => p.PurchaseOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PO_Status");

            entity.HasOne(d => d.Supplier).WithMany(p => p.PurchaseOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PO_Supplier");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.PurchaseOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PO_Warehouse");
        });

        modelBuilder.Entity<PurchaseOrderLine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_POLines");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.LineTotal).HasComputedColumnSql("([QuantityOrdered]*[UnitCost])", true);

            entity.HasOne(d => d.Product).WithMany(p => p.PurchaseOrderLines)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_POLines_Product");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseOrderLines).HasConstraintName("FK_POLines_PO");
        });

        modelBuilder.Entity<PurchaseOrderStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_POStatuses");
        });

        modelBuilder.Entity<ReorderSuggestion>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.GeneratedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Product).WithMany(p => p.ReorderSuggestions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reorder_Product");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.ReorderSuggestions).HasConstraintName("FK_Reorder_PO");

            entity.HasOne(d => d.Severity).WithMany(p => p.ReorderSuggestions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reorder_Severity");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.ReorderSuggestions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reorder_Warehouse");
        });

        modelBuilder.Entity<StockAlert>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Product).WithMany(p => p.StockAlerts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Alerts_Product");

            entity.HasOne(d => d.ReadByUser).WithMany(p => p.StockAlerts).HasConstraintName("FK_Alerts_ReadBy");

            entity.HasOne(d => d.Severity).WithMany(p => p.StockAlerts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Alerts_Severity");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.StockAlerts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Alerts_Warehouse");
        });

        modelBuilder.Entity<StockLevel>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Product).WithMany(p => p.StockLevels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StockLevels_Product");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.StockLevels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StockLevels_Warehouse");
        });

        modelBuilder.Entity<StockMovement>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.StockMovements).HasConstraintName("FK_StockMov_User");

            entity.HasOne(d => d.MovementType).WithMany(p => p.StockMovements)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StockMov_Type");

            entity.HasOne(d => d.Product).WithMany(p => p.StockMovements)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StockMov_Product");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.StockMovements)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StockMov_Warehouse");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<SupplierDelivery>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DaysLate).HasComputedColumnSql("(case when [ActualDate] IS NOT NULL AND [ActualDate]>[PromisedDate] then datediff(day,[PromisedDate],[ActualDate]) else (0) end)", true);
            entity.Property(e => e.IsOnTime).HasComputedColumnSql("(case when [ActualDate] IS NOT NULL AND [ActualDate]<=[PromisedDate] then CONVERT([bit],(1)) else CONVERT([bit],(0)) end)", true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.SupplierDeliveries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Deliveries_PO");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierDeliveries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Deliveries_Supplier");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email, "IX_Users_Email").HasFilter("([IsDeleted]=(0))");

            entity.HasIndex(e => e.RoleId, "IX_Users_RoleId").HasFilter("([IsDeleted]=(0))");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Role");
        });

        modelBuilder.Entity<VelocityCache>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.ComputedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Product).WithMany(p => p.VelocityCaches)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Velocity_Product");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.VelocityCaches)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Velocity_Warehouse");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasIndex(e => e.Code, "IX_Warehouses_Code").HasFilter("([IsDeleted]=(0))");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Manager).WithMany(p => p.Warehouses).HasConstraintName("FK_Warehouses_Manager");
        });

        modelBuilder.Entity<WarehouseTransfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Transfers");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status).HasDefaultValue("Pending");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.DestMovement).WithMany(p => p.WarehouseTransferDestMovements).HasConstraintName("FK_Transfers_DstMovement");

            entity.HasOne(d => d.DestWarehouse).WithMany(p => p.WarehouseTransferDestWarehouses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transfers_Dest");

            entity.HasOne(d => d.InitiatedByUser).WithMany(p => p.WarehouseTransfers).HasConstraintName("FK_Transfers_User");

            entity.HasOne(d => d.Product).WithMany(p => p.WarehouseTransfers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transfers_Product");

            entity.HasOne(d => d.SourceMovement).WithMany(p => p.WarehouseTransferSourceMovements).HasConstraintName("FK_Transfers_SrcMovement");

            entity.HasOne(d => d.SourceWarehouse).WithMany(p => p.WarehouseTransferSourceWarehouses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transfers_Source");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    internal object CreateConnection()
    {
        throw new NotImplementedException();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
