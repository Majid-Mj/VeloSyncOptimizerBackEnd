using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeloSyncOptimizer.Application.Common.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId, int roleId);
        string GenerateRefreshToken();
    }
}
