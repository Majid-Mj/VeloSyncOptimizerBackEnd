
using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;

using VeloSyncOptimizer.Domain.Entities;

namespace VeloSyncOptimizer.Application.Features.Auth.Commands.Register;

public class RegisterUserCommandHandler
    : IRequestHandler<RegisterUserCommand, Guid>
{
    // ✅ Only interfaces — no AppDbContext, no EF Core, no Infrastructure
    private readonly IUserRepository _userRepo;
    private readonly IPasswordService _password;

    public RegisterUserCommandHandler(
        IUserRepository userRepo,
        IPasswordService password)
    {
        _userRepo = userRepo;
        _password = password;
    }

    public async Task<Guid> Handle(RegisterUserCommand req, CancellationToken ct)
    {
        // 1. Check duplicate
        var exists = await _userRepo.ExistsByEmailAsync(req.Email, ct);

        if (exists)
            throw new InvalidOperationException($"Email '{req.Email}' is already registered");


        if (req.RoleId != 2 && req.RoleId != 3)
            throw new Exception("Invalid role selected");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = req.Email.ToLower().Trim(),
            PasswordHash = _password.Hash(req.Password),
            FirstName = req.FirstName,
            LastName = req.LastName,
            RoleId = (byte)req.RoleId,        
            IsActive = true,
            IsApproved = false          // 🔥 KEY CHANGE
        };

        var newId = await _userRepo.CreateAsync(user, ct);

        return newId;
    }
}
