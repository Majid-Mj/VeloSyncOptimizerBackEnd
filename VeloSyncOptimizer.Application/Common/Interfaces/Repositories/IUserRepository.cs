using VeloSyncOptimizer.Infrastructure.Persistence.Models;

namespace VeloSyncOptimizer.Application.Common.Interfaces.Repositories
{

    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken ct);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);
        Task<Guid> CreateAsync(User user, CancellationToken ct);
        Task SaveRefreshTokenAsync(Domain.Entities.RefreshToken token, CancellationToken ct);
    }
}
