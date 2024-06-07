using System.Diagnostics;
using ApplicationEF.Exceptions;
using Newtonsoft.Json;

namespace EWSWebApi;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            ProjectNotFoundException => StatusCodes.Status404NotFound,
            ProjectAccessDeniedException => StatusCodes.Status401Unauthorized,
            InsufficientPermissionsException => StatusCodes.Status403Forbidden,
            ProjectMemberNonExistantException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new
        {
            title = exception.Message,
            statusCode = statusCode
        };
        
        Console.WriteLine($"An error occured: {exception.Message}");

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}