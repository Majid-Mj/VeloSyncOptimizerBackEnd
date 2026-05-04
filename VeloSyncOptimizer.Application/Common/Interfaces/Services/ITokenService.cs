using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeloSyncOptimizer.Application.Common.Interfaces.Services;

public interface ITokenService
{
    string GenerateAccessToken(int userId, string email, string roleName, int roleId);
    string GenerateRefreshToken();
}

