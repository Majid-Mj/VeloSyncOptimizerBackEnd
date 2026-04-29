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
        }
        catch (Exception ex)
        {

            var errorMessage = ex.InnerException?.Message ?? ex.Message;

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";


            var response = new ApiResponse<string>
            {
                Success = false,
                Message = "An error occurred",
                Errors = new List<string> { errorMessage }
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
