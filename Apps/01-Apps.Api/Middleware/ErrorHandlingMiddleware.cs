using System.Net;
using System.Text.Json;

namespace Apps.Api.Middleware;
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    //Entry point
    public async Task Invoke(HttpContext context)
    {
        try
        {
          //Will execute the next middleware in the pipeline - from Program.cs -> app.MapControllers(); // app.UseHttpsRedirection();
          await _next(context);
        }
        catch (Exception ex)
        {
          //Will be executed when error occurs
          await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = JsonSerializer.Serialize(new { error = "An Error Occured while processing your request" });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}