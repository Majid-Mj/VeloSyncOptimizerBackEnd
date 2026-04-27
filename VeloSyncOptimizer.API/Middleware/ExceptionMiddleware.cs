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
            var errorMessage = ex.InnerException?.Message ?? ex.Message;

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