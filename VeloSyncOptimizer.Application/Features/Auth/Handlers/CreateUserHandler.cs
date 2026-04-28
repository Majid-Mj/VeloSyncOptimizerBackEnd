using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;
using VeloSyncOptimizer.Application.Features.Auth.Commands;
using VeloSyncOptimizer.Infrastructure.Persistence.Models;

namespace VeloSyncOptimizer.Application.Features.Auth.Handlers;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepo;
    private readonly IPasswordService _password;

    public CreateUserHandler(
        IUserRepository userRepo,
        IPasswordService password)
    {
        _userRepo = userRepo;
        _password = password;
    }

    public async Task<Guid> Handle(CreateUserCommand req, CancellationToken ct)
    {
        var exists = await _userRepo.ExistsByEmailAsync(req.Email, ct);

        if (exists)
            throw new InvalidOperationException($"Email '{req.Email}' is already registered");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = req.Email.ToLower().Trim(),
            PasswordHash = _password.Hash(req.Password),
            FirstName = req.FirstName,
            LastName = req.LastName,
            RoleId = req.RoleId, 
            IsActive = true
        };

        var newId = await _userRepo.CreateAsync(user, ct);
        return newId;
    }
}
