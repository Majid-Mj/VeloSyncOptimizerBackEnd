using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeloSyncOptimizer.Application.Common.Helpers;
using VeloSyncOptimizer.Application.Features.Auth.Commands;
using VeloSyncOptimizer.Application.Features.Auth.Queries;

namespace VeloSyncOptimizer.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [Authorize(Roles = "Administrator")]
    [HttpGet("pending")]
    public async Task<IActionResult> GetPending(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetPendingUsersQuery(), ct);
        return Ok(ResponseFactory.Success(result, "Pending users retrieved successfully"));
    }


    [Authorize(Roles = "Administrator")]
    [HttpPut("approve/{id}")]
    public async Task<IActionResult> Approve(int id, CancellationToken ct)
    {
        await _mediator.Send(new ApproveUserCommand(id), ct);
        return Ok(ResponseFactory.Success(new { id }, "User approved successfully"));
    }
}

