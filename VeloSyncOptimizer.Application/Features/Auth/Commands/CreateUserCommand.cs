using MediatR;

namespace VeloSyncOptimizer.Application.Features.Auth.Commands;

public record CreateUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    byte RoleId
) : IRequest<Guid>;
