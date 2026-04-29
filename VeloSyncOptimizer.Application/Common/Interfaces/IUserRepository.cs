

using VeloSyncOptimizer.Application.Features.Users.DTOs;
using VeloSyncOptimizer.Infrastructure.Persistence.Models;

namespace VeloSyncOptimizer.Application.Common.Interfaces
{

    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken ct);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);
        Task<Guid> CreateAsync(User user, CancellationToken ct);
        Task SaveRefreshTokenAsync(VeloSyncOptimizer.Domain.Entities.RefreshToken token, CancellationToken ct);

        Task<User?> GetByIdAsync(Guid id);
        Task SaveChangesAsync(CancellationToken cancellationToken);

        Task<List<UserDto>> GetPendingUsersAsync(CancellationToken ct);
    }
}
