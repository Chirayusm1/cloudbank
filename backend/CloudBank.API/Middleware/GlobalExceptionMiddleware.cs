using System.Text.Json;

namespace CloudBank.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (KeyNotFoundException ex)
        {
            await WriteErrorResponse(
                context,
                StatusCodes.Status404NotFound,
                ex.Message);
        }
        catch (ArgumentException ex)
        {
            await WriteErrorResponse(
                context,
                StatusCodes.Status400BadRequest,
                ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await WriteErrorResponse(
                context,
                StatusCodes.Status400BadRequest,
                ex.Message);
        }
    }

    private static async Task WriteErrorResponse(
        HttpContext context,
        int statusCode,
        string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = statusCode,
            message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}
