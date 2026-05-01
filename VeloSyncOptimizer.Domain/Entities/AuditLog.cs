using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSyncOptimizer.Domain.Entities;

[Table("AuditLogs", Schema = "audit")]
[Index("EntityName", "EntityId", "CreatedAt", Name = "IX_AuditLogs_Entity", IsDescending = new[] { false, false, true })]
[Index("UserId", "CreatedAt", Name = "IX_AuditLogs_User", IsDescending = new[] { false, true })]
public partial class AuditLog
{
    [Key]
    public long Id { get; set; }

    public int? UserId { get; set; }

    [StringLength(100)]
    public string Action { get; set; } = null!;

    [StringLength(100)]
    public string EntityName { get; set; } = null!;

    [StringLength(100)]
    public string EntityId { get; set; } = null!;

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    [StringLength(50)]
    public string? IpAddress { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("AuditLogs")]
    public virtual User? User { get; set; }
}
