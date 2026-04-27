using MediatR;
using VeloSyncOptimizer.Application.Features.Auth.DTOs;

namespace VeloSyncOptimizer.Application.Features.Auth.Commands
{
    public record LoginUserCommand(
     string Email,
     string Password
        ) : IRequest<AuthResponseDto>;
}
