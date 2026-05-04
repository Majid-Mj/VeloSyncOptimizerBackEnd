using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeloSyncOptimizer.Application.Common.Helpers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<IActionResult> GetProducts(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken cancellationToken = default)
    {
        var query = new GetProductsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query, cancellationToken);

        return Ok(ResponseFactory.Success(result, "Products fetched successfully"));
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(
    int id,
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetProductByIdQuery { Id = id },
            cancellationToken);

        if (result == null)
            return NotFound("Product not found");

        return Ok(ResponseFactory.Success(result, "Product fetched successfully"));
    }

}