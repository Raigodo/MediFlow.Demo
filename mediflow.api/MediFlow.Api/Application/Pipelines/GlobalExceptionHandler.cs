using System.Net;
using System.Text.Json;

namespace MediFlow.Api.Application.Pipelines;

public class GlobalExceptionHandler(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (BadHttpRequestException e)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(new
            {
                Message = "Problem with provided data",
                Exception = e.Message,
            });
        }
        catch (Exception ex)
        {
            await HandleGeneralExceptionAsync(httpContext, ex);
        }
    }


    private static async Task HandleGeneralExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var result = JsonSerializer.Serialize(new
        {
            context.Response.StatusCode,
            Message = "An unexpected error occurred",
            Detailed = exception.Message
        });

        await context.Response.WriteAsync(result);
    }
}
