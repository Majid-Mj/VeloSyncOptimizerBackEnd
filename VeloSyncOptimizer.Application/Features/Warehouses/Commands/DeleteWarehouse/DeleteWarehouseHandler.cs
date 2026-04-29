using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;

public class DeleteWarehouseHandler
    : IRequestHandler<DeleteWarehouseCommand, bool>
{
    private readonly IWarehouseRepository _repo;

    public DeleteWarehouseHandler(IWarehouseRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> Handle(DeleteWarehouseCommand request, CancellationToken ct)
    {
        return await _repo.SoftDeleteAsync(request.Id, ct);
    }
}