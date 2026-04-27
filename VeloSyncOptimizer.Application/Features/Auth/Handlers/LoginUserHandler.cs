
using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces;
using VeloSyncOptimizer.Application.Features.Auth.Commands;
using VeloSyncOptimizer.Application.Features.Auth.DTOs;

namespace VeloSyncOptimizer.Application.Features.Auth.Handlers;

public class LoginUserHandler
    : IRequestHandler<LoginUserCommand, AuthResponseDto>
{
    // ✅ Only interfaces — no EF Core, no Dapper, no AppDbContext
    private readonly IUserRepository _userRepo;
    private readonly IPasswordService _password;
    private readonly IJwtService _jwt;

    public LoginUserHandler(
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
        var user = await _userRepo.GetByEmailAsync(req.Email, ct);

        // 2. Validate credentials
        if (user is null || !_password.Verify(req.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password");

        // 3. Generate JWT via interface
        var roleName = user.RoleId switch
        {
            1 => "Administrator",
            2 => "WarehouseManager",
            3 => "ProcurementOfficer",
            _ => "User"
        };

        var accessToken = _jwt.GenerateToken(
            user.Id,
            user.Email,
            roleName
        );

        var refreshToken = _jwt.GenerateRefreshToken();

        // 4. Save Refresh Token to DB
        await _userRepo.SaveRefreshTokenAsync(new VeloSyncOptimizer.Domain.Entities.RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        }, ct);

        // 5. Return response DTO
        return new AuthResponseDto
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            Email = user.Email,
            Role = roleName,
            Expiry = DateTime.UtcNow.AddHours(2)
        };
    }
}