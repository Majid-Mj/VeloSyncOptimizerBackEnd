using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSyncOptimizer.Infrastructure.Persistence.Models;

[Table("PurchaseOrderStatuses", Schema = "procurement")]
[Index("Name", Name = "UQ_POStatuses_Name", IsUnique = true)]
public partial class PurchaseOrderStatus
{
    [Key]
    public byte Id { get; set; }

    [StringLength(20)]
    public string Name { get; set; } = null!;

    [InverseProperty("Status")]
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}
