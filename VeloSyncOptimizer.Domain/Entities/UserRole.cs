using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSyncOptimizer.Infrastructure.Persistence.Models;

[Table("UserRoles", Schema = "identity")]
[Index("Name", Name = "UQ_UserRoles_Name", IsUnique = true)]
public partial class UserRole
{
    [Key]
    public byte Id { get; set; }

    [StringLength(30)]
    public string Name { get; set; } = null!;

    [InverseProperty("Role")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
