using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeloSyncOptimizer.Application.Common.Helpers;
using VeloSyncOptimizer.Application.Features.Auth.Commands.Login;
using VeloSyncOptimizer.Application.Features.Auth.Commands.Register;



using VeloSyncOptimizer.Application.Features.Auth.DTOs;

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


    [AllowAnonymous]
    [HttpPost("register")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Register([FromForm] RegisterRequestDto dto)
    {
        var command = new RegisterUserCommand(
            dto.Email,
            dto.Password,
            dto.FirstName,
            dto.LastName,
            dto.Role
        );

        var id = await _mediator.Send(command);

        return Ok(ResponseFactory.Success(new { UserId = id }, "User registered"));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Login([FromForm] LoginUserCommand command)
    {
        var result = await _mediator.Send(command);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,          // ⚠️ Set true in production
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddHours(2)
        };

        Response.Cookies.Append("AccessToken", result.AccessToken, cookieOptions);

        var refreshCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,          // ⚠️ Set true in production
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("RefreshToken", result.RefreshToken, refreshCookieOptions);

        return Ok(ResponseFactory.Success(result, "Login successful"));
    }

    //[HttpPost("create-user")]
    //[Microsoft.AspNetCore.Authorization.Authorize(Roles = "Administrator")]
    //public async Task<IActionResult> CreateUser(CreateUserCommand command)
    //{
    //    var id = await _mediator.Send(command);
    //    return Ok(ResponseFactory.Success(new { UserId = id }, "User created successfully"));
    //}



    //Logout Controller

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        // Automatically get RefreshToken from cookies
        Request.Cookies.TryGetValue("RefreshToken", out var refreshToken);

        if (!string.IsNullOrEmpty(refreshToken))
        {
            await _mediator.Send(new LogoutCommand(refreshToken), ct);
        }                           

        // Clear auth cookies
        Response.Cookies.Delete("AccessToken");
        Response.Cookies.Delete("RefreshToken");

        return Ok(ResponseFactory.Success("Logged out successfully", "Success"));
    }


    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(CancellationToken ct)
    {
        // Automatically get RefreshToken from cookies
        if (!Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
        {
            return Unauthorized(ResponseFactory.Error("No refresh token found in cookies"));
        }

        var result = await _mediator.Send(new RefreshTokenCommand(refreshToken), ct);

        // Update Cookies with new tokens
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(2)
        };
        Response.Cookies.Append("AccessToken", result.AccessToken, cookieOptions);

        var refreshCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("RefreshToken", result.RefreshToken, refreshCookieOptions);

        return Ok(ResponseFactory.Success(result, "Token refreshed successfully"));
    }
}
