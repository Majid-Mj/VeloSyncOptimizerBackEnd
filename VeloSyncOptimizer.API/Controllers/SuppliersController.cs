using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeloSyncOptimizer.Application.Common.Helpers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SuppliersController : ControllerBase
{
    private readonly IMediator _mediator;

    public SuppliersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSuppliers(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllSuppliersQuery(), ct);

        return Ok(ResponseFactory.Success(result, "Suppliers fetched successfully"));
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetSupplierById(
    Guid id,
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetSupplierByIdQuery { Id = id },
            cancellationToken);

        return Ok(ResponseFactory.Success(result, "Supplier fetched successfully"));
    }


    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> CreateSupplier(
    [FromBody] CreateSupplierCommand command,
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(ResponseFactory.Success(result, "Supplier created successfully"));
    }
}