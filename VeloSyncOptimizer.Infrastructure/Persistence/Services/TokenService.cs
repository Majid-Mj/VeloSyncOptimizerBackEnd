using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeloSyncOptimizer.Infrastructure.Persistence.Services;

using System.Security.Cryptography;
using VeloSyncOptimizer.Application.Common.Interfaces.Services;

public class TokenService : ITokenService
{
    private readonly IJwtService _jwt;

    public TokenService(IJwtService jwt)
    {
        _jwt = jwt;
    }

    public string GenerateAccessToken(int userId, string roleName)
        => _jwt.GenerateToken(userId, roleName);

    public string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
}
