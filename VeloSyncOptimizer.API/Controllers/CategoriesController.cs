using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeloSyncOptimizer.Application.Common.Helpers;

namespace VeloSyncOptimizer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllCategoriesQuery(), cancellationToken);

        return Ok(ResponseFactory.Success(result, "Categories fetched successfully"));
    }
}