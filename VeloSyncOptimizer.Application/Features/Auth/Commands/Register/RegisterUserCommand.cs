
using MediatR;

namespace VeloSyncOptimizer.Application.Features.Auth.Commands.Register;

public record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    int RoleId  
) : IRequest<Guid>;