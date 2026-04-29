using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces;
using VeloSyncOptimizer.Application.Features.Users.DTOs;

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