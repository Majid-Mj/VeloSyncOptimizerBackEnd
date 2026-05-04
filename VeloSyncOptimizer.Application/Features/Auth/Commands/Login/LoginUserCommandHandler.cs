
using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;
using VeloSyncOptimizer.Application.Common.Interfaces.Services;
using VeloSyncOptimizer.Application.Features.Auth.DTOs;

namespace VeloSyncOptimizer.Application.Features.Auth.Commands.Login;

public class LoginUserCommandHandler
    : IRequestHandler<LoginUserCommand, AuthResponseDto>
{
    // ✅ Only interfaces — no EF Core, no Dapper, no AppDbContext
    private readonly IUserRepository _userRepo;
    private readonly IPasswordService _password;
    private readonly IJwtService _jwt;

    public LoginUserCommandHandler(
        IUserRepository userRepo,
        IPasswordService password,
        IJwtService jwt)
    {
        _userRepo = userRepo;
        _password = password;
        _jwt = jwt;
    }

    public async Task<AuthResponseDto> Handle(
        LoginUserCommand req, CancellationToken ct)
    {
        // 1. Fetch user via interface — Dapper runs behind the scenes
        var user = await _userRepo.GetByEmailAsync(req.Email.ToLower(), ct);

        // 2. Validate credentials
        if (user is null || !_password.Verify(req.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password");

        if (!user.IsApproved)
            throw new Exception("Account not approved by admin");

        // 3. Generate JWT via interface
        var accessToken = _jwt.GenerateToken(
            user.Id,
            user.Email,
            user.RoleName,
            user.RoleId
        );

        var refreshToken = _jwt.GenerateRefreshToken();

        // 4. Save Refresh Token to DB
        await _userRepo.SaveRefreshTokenAsync(new VeloSyncOptimizer.Domain.Entities.RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            UserName = user.FirstName + " " + user.LastName,
            RoleId = user.RoleId,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        }, ct);

        // 5. Return response DTO
        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            RoleId = user.RoleId,
            Expiry = DateTime.UtcNow.AddHours(2)
        };
    }
}
