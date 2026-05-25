using System.Net;
using System.Text.Json;

namespace SilkRoad.API;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _enviroment;

    public ExceptionMiddleware(RequestDelegate next, IHostEnvironment enviroment)
    {
        _next = next;
        _enviroment = enviroment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            PreventXssAndCSRFAttack(context);
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            APIException response = _enviroment.IsDevelopment() ?
            new APIException
            (
                (int)HttpStatusCode.InternalServerError,
                ex.Message,
                ex.StackTrace
            ) :
            new APIException
            (
                (int)HttpStatusCode.InternalServerError,
                ex.Message
            );
            string json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }

    private void PreventXssAndCSRFAttack(HttpContext context)
    {
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Append("X-Frame-Options", "DENY");
        context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    }
}
