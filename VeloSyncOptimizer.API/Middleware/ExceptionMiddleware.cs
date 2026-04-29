using VeloSyncOptimizer.Application.Common.Models;

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
<<<<<<< HEAD
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
=======
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
>>>>>>> origin/main

            var response = new ApiResponse<string>
            {
                Success = false,
                Message = "An error occurred",
<<<<<<< HEAD
                Errors = new List<string> { errorMessage }
=======
                Errors = new List<string> { ex.Message }
>>>>>>> origin/main
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}