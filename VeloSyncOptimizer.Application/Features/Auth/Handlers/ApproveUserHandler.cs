using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces;
using VeloSyncOptimizer.Application.Features.Auth.Commands;

public class ApproveUserHandler : IRequestHandler<ApproveUserCommand>
{
    private readonly IUserRepository _repo;

    public ApproveUserHandler(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(ApproveUserCommand request, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(request.UserId);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        user.IsApproved = true;
        user.UpdatedAt = DateTime.UtcNow;

        await _repo.SaveChangesAsync(ct);
    }
}