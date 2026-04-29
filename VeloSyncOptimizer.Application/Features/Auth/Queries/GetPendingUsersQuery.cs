using MediatR;
using VeloSyncOptimizer.Application.Features.Auth.DTOs;
using VeloSyncOptimizer.Application.Features.Users.DTOs;

public record GetPendingUsersQuery() : IRequest<List<UserDto>>;