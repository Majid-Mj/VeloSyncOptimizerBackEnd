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

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> CreateCategory(
    [FromBody] CreateCategoryCommand command,
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(ResponseFactory.Success(result, "Category created successfully"));
    }



    [HttpPut("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateCategory(
     Guid id,
     [FromBody] UpdateCategoryRequest request,
     CancellationToken cancellationToken)
    {
        var command = new UpdateCategoryCommand
        {
            Id = id,
            Name = request.Name,
            ParentId = request.ParentId
        };

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(ResponseFactory.Success(result, "Updated successfully"));
    }



    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteCategory(
    Guid id,
    CancellationToken cancellationToken)
    {
        var command = new DeleteCategoryCommand
        {
            Id = id
        };

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(ResponseFactory.Success(result, "Category deleted successfully"));
    }


}