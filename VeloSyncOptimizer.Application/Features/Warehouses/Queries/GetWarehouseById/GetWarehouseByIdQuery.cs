using MediatR;
using VeloSyncOptimizer.Application.Features.Warehouses.DTOs;


public record GetWarehouseByIdQuery(Guid Id)
    : IRequest<WarehouseDto?>; 