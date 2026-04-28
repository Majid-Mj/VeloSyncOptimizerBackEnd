using MediatR;
using VeloSyncOptimizer.Application.Features.Auth.DTOs;

namespace VeloSyncOptimizer.Application.Features.Auth.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement current user logic (e.g. from ICurrentUserService or Claims)
        return await Task.FromResult(new AuthResponseDto());
    }
}
