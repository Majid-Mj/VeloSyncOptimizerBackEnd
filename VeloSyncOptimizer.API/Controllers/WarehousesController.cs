using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeloSyncOptimizer.Application.Common.Helpers;
using VeloSyncOptimizer.Application.Features.Warehouses.Commands.CreateWarehouse;
using VeloSyncOptimizer.Application.Features.Warehouses.DTOs;
using VeloSyncOptimizer.Application.Features.Warehouses.Queries.GetAllWarehouses;

namespace VeloSyncOptimizer.API.Controllers;

[ApiController]
[Route("api/warehouses")]
[Authorize]
public class WarehousesController : ControllerBase
{
    private readonly IMediator _mediator;

    public WarehousesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// GET api/warehouses
    /// Returns all non-deleted warehouses via [inventory].[sp_GetWarehouses].
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllWarehousesQuery(), cancellationToken);

        return Ok(ResponseFactory.Success(result, "Warehouses retrieved successfully"));
    }
    /// <summary>
    /// POST api/warehouses
    /// Creates a new warehouse via EF Core and returns 201 Created with the new Id.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Create(
        [FromBody] CreateWarehouseCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return Ok(ResponseFactory.Success(new { id }, "Warehouse created successfully"));
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetWarehouseByIdQuery(id), ct);

        if (result == null)
            return NotFound(
                ResponseFactory.Failure<WarehouseDto>("Warehouse not found")
            );

        return Ok(
            ResponseFactory.Success(result, "Warehouse retrieved successfully")
        );
    }


    [Authorize(Roles = "Administrator")]
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(
    Guid id,
    UpdateWarehouseRequestDto dto,
    CancellationToken ct)
    {
        var command = new UpdateWarehouseCommand(
            id,
            dto.Code,
            dto.Name,
            dto.City,
            dto.State,
            dto.Country,
            dto.TotalCapacity,
            dto.IsActive
        );

        var result = await _mediator.Send(command, ct);

        if (!result)
            return NotFound(
                ResponseFactory.Failure<object>("Warehouse not found or not updated")
            );

        return Ok(
            ResponseFactory.Success<object>(null, "Warehouse updated successfully")
        );
    }


    /// <summary>
    /// PUT api/warehouses/{id}
    /// Full replacement — all fields are required. Untouched fields will be overwritten.
    /// </summary>
    [Authorize(Roles = "Administrator")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Replace(
        Guid id,
        [FromBody] ReplaceWarehouseRequestDto dto,
        CancellationToken ct)
    {
        // Build a full update command with all fields explicitly set
        var command = new UpdateWarehouseCommand(
            id,
            dto.Code,
            dto.Name,
            dto.City,
            dto.State,
            dto.Country,
            dto.TotalCapacity,
            dto.IsActive
        );

        var result = await _mediator.Send(command, ct);

        if (!result)
            return NotFound(
                ResponseFactory.Failure<object>("Warehouse not found or already deleted")
            );

        return Ok(
            ResponseFactory.Success<object>(null, "Warehouse replaced successfully")
        );
    }


    [Authorize(Roles = "Administrator")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> SoftDelete(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new DeleteWarehouseCommand(id), ct);

        if (!result)
            return NotFound(
                ResponseFactory.Failure<object>("Warehouse not found or already deleted")
            );

        return Ok(
            ResponseFactory.Success<object>(null, "Warehouse deleted successfully")
        );
    }


}

