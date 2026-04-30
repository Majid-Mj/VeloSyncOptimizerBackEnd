using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;
using VeloSyncOptimizer.Application.Common.Interfaces.Services;
using VeloSyncOptimizer.Domain.Entities;

public class RefreshTokenHandler
    : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepo;
    private readonly IJwtService _jwtService;

    public RefreshTokenHandler(IUserRepository userRepo, IJwtService jwtService)
    {
        _userRepo = userRepo;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto>Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var storedToken = await _userRepo.GetRefreshTokenAsync(request.RefreshToken, ct);

        if (storedToken == null)
            throw new Exception("Invalid refresh token");

        if (storedToken.IsRevoked)
            throw new Exception("Token revoked");

        if (storedToken.ExpiresAt < DateTime.UtcNow)
            throw new Exception("Token expired");

        var user = await _userRepo.GetByIdAsync(storedToken.UserId);

        if (user == null)
            throw new Exception("User not found");

        // 🔥 Generate new JWT
        var newAccessToken = _jwtService.GenerateToken(user.Id, user.RoleName);

        // 🔥 Rotate refresh token (recommended)
        storedToken.IsRevoked = true;

        var newRefreshToken = _jwtService.GenerateRefreshToken();

        await _userRepo.SaveRefreshTokenAsync(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        }, ct);

        await _userRepo.SaveChangesAsync(ct);

        return new AuthResponseDto
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken,
            RoleId = user.RoleId,
            Expiry = DateTime.UtcNow.AddMinutes(30)
        };
    }
}