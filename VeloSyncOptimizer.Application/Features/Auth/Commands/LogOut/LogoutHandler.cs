using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;

public class LogoutHandler : IRequestHandler<LogoutCommand>
{
    private readonly IUserRepository _userRepo;

    public LogoutHandler(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task Handle(LogoutCommand request, CancellationToken ct)
    {
        await _userRepo.RevokeRefreshTokenAsync(request.RefreshToken, ct);
    }
}