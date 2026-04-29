using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeloSyncOptimizer.Application.Common.Helpers;
using VeloSyncOptimizer.Application.Features.Auth.Commands;

namespace VeloSyncOptimizer.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(ResponseFactory.Success(new { UserId = id }, "User registered"));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginUserCommand command)
    {
        var result = await _mediator.Send(command);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(2)
        };

        Response.Cookies.Append("AccessToken", result.Token, cookieOptions);

        var refreshCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("RefreshToken", result.RefreshToken, refreshCookieOptions);

        return Ok(ResponseFactory.Success(result, "Login successful"));
    }

    [HttpPost("create-user")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Administrator")]
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(ResponseFactory.Success(new { UserId = id }, "User created successfully"));
    }
}
