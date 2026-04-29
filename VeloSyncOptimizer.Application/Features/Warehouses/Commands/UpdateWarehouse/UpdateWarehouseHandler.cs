using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;

public class UpdateWarehouseHandler
    : IRequestHandler<UpdateWarehouseCommand, bool>
{
    private readonly IWarehouseRepository _repo;

    public UpdateWarehouseHandler(IWarehouseRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> Handle(UpdateWarehouseCommand request, CancellationToken ct)
    {
        var existing = await _repo.GetByIdAsync(request.Id, ct);

        if (existing == null)
            return false;


        existing.Code = request.Code ?? existing.Code;
        existing.Name = request.Name ?? existing.Name;

        existing.City = request.City ?? existing.City;
        existing.State = request.State ?? existing.State;
        existing.Country = request.Country ?? existing.Country;

        existing.TotalCapacity = request.TotalCapacity ?? existing.TotalCapacity;
        existing.IsActive = request.IsActive ?? existing.IsActive;

        return await _repo.UpdateAsync(existing, ct);
    }
}