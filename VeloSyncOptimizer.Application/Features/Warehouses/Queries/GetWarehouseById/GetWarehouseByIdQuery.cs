using MediatR;
using VeloSyncOptimizer.Application.Features.Warehouses.DTOs;


public record GetWarehouseByIdQuery(int Id)
    : IRequest<WarehouseDto?>;