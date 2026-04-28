using MediatR;
using VeloSyncOptimizer.Application.Features.Auth.DTOs;

namespace VeloSyncOptimizer.Application.Features.Auth.Queries.GetCurrentUser;

public record GetCurrentUserQuery : IRequest<AuthResponseDto>;
