using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;
using VeloSyncOptimizer.Application.Common.Interfaces.Services;
using VeloSyncOptimizer.Domain.Entities;

namespace VeloSyncOptimizer.Application.Features.Auth.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
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

    public async Task<int> Handle(CreateUserCommand req, CancellationToken ct)
    {
        var email = req.Email.ToLower();

        var exists = await _userRepo.ExistsByEmailAsync(email, ct);

        if (exists)
            throw new InvalidOperationException($"Email '{email}' is already registered");

        var user = new User
        {
            Email = email,
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
