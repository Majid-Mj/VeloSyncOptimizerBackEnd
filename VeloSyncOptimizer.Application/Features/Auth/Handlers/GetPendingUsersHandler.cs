using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;
using VeloSyncOptimizer.Application.Features.Users.DTOs;
using VeloSyncOptimizer.Application.Features.Auth.Queries;

namespace VeloSyncOptimizer.Application.Features.Auth.Handlers;

public class GetPendingUsersHandler
    : IRequestHandler<GetPendingUsersQuery, List<UserDto>>
{
    private readonly IUserRepository _userRepo;

    public GetPendingUsersHandler(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<List<UserDto>> Handle(GetPendingUsersQuery request, CancellationToken ct)
    {
        return await _userRepo.GetPendingUsersAsync(ct);
    }
}
