using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;

using VeloSyncOptimizer.Domain.Entities;

namespace VeloSyncOptimizer.Application.Features.Auth.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepo;
    private readonly IPasswordService _password;

    public CreateUserCommandHandler(
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
            IsActive = true,
            IsApproved = true
        };

        var newId = await _userRepo.CreateAsync(user, ct);
        return newId;
    }
}
