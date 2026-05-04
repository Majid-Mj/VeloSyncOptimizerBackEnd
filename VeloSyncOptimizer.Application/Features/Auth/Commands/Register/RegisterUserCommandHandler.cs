
using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;
using VeloSyncOptimizer.Application.Common.Interfaces.Services;
using VeloSyncOptimizer.Domain.Entities;
using VeloSyncOptimizer.Domain.Enums;

namespace VeloSyncOptimizer.Application.Features.Auth.Commands.Register;

public class RegisterUserCommandHandler
    : IRequestHandler<RegisterUserCommand, int>
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

    public async Task<int> Handle(RegisterUserCommand req, CancellationToken ct)
    {
        var email = req.Email.ToLower();

        // 1. Check duplicate
        var exists = await _userRepo.ExistsByEmailAsync(email, ct);

        if (exists)
            throw new InvalidOperationException($"Email '{email}' is already registered");


        // Removed manual role checks as Enum handles it
        if (req.Role == UserRoleEnum.Administrator)
            throw new InvalidOperationException("Registration as Administrator is not allowed.");
        
        var user = new User
        {
            Email = email,
            PasswordHash = _password.Hash(req.Password),
            FirstName = req.FirstName,
            LastName = req.LastName,
            RoleId = (byte)req.Role,        
            IsActive = true,
            IsApproved = false          
        };

        var newId = await _userRepo.CreateAsync(user, ct);

        return newId;
    }
}
