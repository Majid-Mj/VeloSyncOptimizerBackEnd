using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeloSyncOptimizer.Application.Common.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string roleName);
        string GenerateRefreshToken();
    }
}
