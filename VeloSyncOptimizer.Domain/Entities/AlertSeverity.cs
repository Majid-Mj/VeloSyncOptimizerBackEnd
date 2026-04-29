using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSyncOptimizer.Domain.Entities;

[Table("AlertSeverities", Schema = "inventory")]
public partial class AlertSeverity
{
    [Key]
    public byte Id { get; set; }

    [StringLength(20)]
    public string Name { get; set; } = null!;

    [InverseProperty("Severity")]
    public virtual ICollection<ReorderSuggestion> ReorderSuggestions { get; set; } = new List<ReorderSuggestion>();

    [InverseProperty("Severity")]
    public virtual ICollection<StockAlert> StockAlerts { get; set; } = new List<StockAlert>();
}
