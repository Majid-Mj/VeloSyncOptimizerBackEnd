using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeloSyncOptimizer.Application.Features.Warehouses.Commands.CreateWarehouse;
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
        return Ok(result);
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
        return CreatedAtAction(nameof(GetAll), new { }, new { id });
    }
}
