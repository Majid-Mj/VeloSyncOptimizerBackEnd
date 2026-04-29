using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSyncOptimizer.Domain.Entities;

[Table("MovementTypes", Schema = "inventory")]
[Index("Name", Name = "UQ_MovementTypes_Name", IsUnique = true)]
public partial class MovementType
{
    [Key]
    public byte Id { get; set; }

    [StringLength(30)]
    public string Name { get; set; } = null!;

    [InverseProperty("MovementType")]
    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}
