
using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;
using VeloSyncOptimizer.Application.Features.Auth.Commands;
using VeloSyncOptimizer.Infrastructure.Persistence.Models;

namespace VeloSyncOptimizer.Application.Features.Auth.Handlers;

public class RegisterUserHandler
    : IRequestHandler<RegisterUserCommand, Guid>
{
    // ✅ Only interfaces — no AppDbContext, no EF Core, no Infrastructure
    private readonly IUserRepository _userRepo;
    private readonly IPasswordService _password;

    public RegisterUserHandler(
        IUserRepository userRepo,
        IPasswordService password)
    {
        _userRepo = userRepo;
        _password = password;
    }

    public async Task<Guid> Handle(RegisterUserCommand req, CancellationToken ct)
    {
        // 1. Check duplicate email — via interface
        var exists = await _userRepo.ExistsByEmailAsync(req.Email, ct);

        if (exists)
            throw new InvalidOperationException($"Email '{req.Email}' is already registered");

        // 2. Build Domain entity — pure C#, no DB concern
        var user = new User
        {
            Id = Guid.NewGuid(),          // ✅ GUID pre-generated here    
            Email = req.Email.ToLower().Trim(),
            PasswordHash = _password.Hash(req.Password),
            FirstName = req.FirstName,
            LastName = req.LastName,
            RoleId = 2, // Default to WarehouseManager or appropriate role id
            IsActive = true
        };

        // 3. Persist via interface — Dapper runs behind the scenes
        var newId = await _userRepo.CreateAsync(user, ct);

        return newId;
    }
}