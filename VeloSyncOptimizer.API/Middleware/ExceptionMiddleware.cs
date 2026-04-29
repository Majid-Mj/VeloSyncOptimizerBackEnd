using VeloSyncOptimizer.Application.Common.Models;
using Microsoft.AspNetCore.Http;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);

            // ✅ Intercept 401/403 AFTER the pipeline runs (auth sets status but no body)
            if (!context.Response.HasStarted)
            {
                if (context.Response.StatusCode == 401)
                {
                    context.Response.ContentType = "application/json";
                    var response = new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Unauthorized: You must be logged in to access this resource.",
                        Data = null,
                        Errors = new List<string> { "Authentication token is missing or invalid." }
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }
                else if (context.Response.StatusCode == 403)
                {
                    context.Response.ContentType = "application/json";
                    var response = new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Forbidden: You do not have permission to perform this action.",
                        Data = null,
                        Errors = new List<string> { "Access denied. Required role not present in token." }
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            var response = new ApiResponse<object>
            {
                Success = false,
                Message = "Unauthorized",
                Data = null,
                Errors = new List<string> { ex.Message }
            };
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.InnerException?.Message ?? ex.Message;

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var response = new ApiResponse<object>
            {
                Success = false,
                Message = "An error occurred",
                Data = null,
                Errors = new List<string> { errorMessage }
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
