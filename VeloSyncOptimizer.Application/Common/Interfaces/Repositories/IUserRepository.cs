using VeloSyncOptimizer.Application.Features.Users.DTOs;
using VeloSyncOptimizer.Domain.Entities;

namespace VeloSyncOptimizer.Application.Common.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken ct);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);
        Task<Guid> CreateAsync(User user, CancellationToken ct);

        Task SaveRefreshTokenAsync(RefreshToken token, CancellationToken ct);

        Task<User?> GetByIdAsync(Guid id);
        Task SaveChangesAsync(CancellationToken cancellationToken);

        Task<List<UserDto>> GetPendingUsersAsync(CancellationToken ct);


        Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken ct);
    }
}
