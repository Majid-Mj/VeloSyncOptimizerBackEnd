using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;

namespace VeloSyncOptimizer.Domain.Entities;

[Table("RefreshTokens", Schema = "identity")]
public class RefreshToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public string Token { get; set; }

    public string UserName { get; set; } = string.Empty;
    public int RoleId { get; set; }

    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }

    public DateTime CreatedAt { get; set; }
}
