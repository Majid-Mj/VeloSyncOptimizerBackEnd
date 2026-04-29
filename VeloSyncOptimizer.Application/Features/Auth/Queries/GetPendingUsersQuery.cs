using MediatR;
using VeloSyncOptimizer.Application.Features.Users.DTOs;

namespace VeloSyncOptimizer.Application.Features.Auth.Queries;

public record GetPendingUsersQuery() : IRequest<List<UserDto>>;
