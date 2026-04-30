using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;
using VeloSyncOptimizer.Application.Features.Warehouses.DTOs;

public class GetWarehouseByIdHandler
    : IRequestHandler<GetWarehouseByIdQuery, WarehouseDto?>
{
    private readonly IWarehouseRepository _repo;

    public GetWarehouseByIdHandler(IWarehouseRepository repo)
    {
        _repo = repo;
    }

    public async Task<WarehouseDto?> Handle(
        GetWarehouseByIdQuery request,
        CancellationToken ct)
    {
        return await _repo.GetByIdAsync(request.Id, ct);
    }
}