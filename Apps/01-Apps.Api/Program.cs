using Apps.Application;
//using Apps.Api.Middleware;
using Apps.Api.Filters;

using Apps.Infrastructure;
using System.ComponentModel;
using Microsoft.AspNetCore.Diagnostics;
// using Apps.Api.Errors;
// using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
  builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    ;
    builder.Services.AddControllers();
    
    // builder.Services.AddSingleton<ProblemDetailsFactory,AppsProblemDetailsFactory>();
}


var app = builder.Build();
{
  app.UseExceptionHandler("/error");
  
  // app.Map("/error", (HttpContext httpContext) =>
  // {
  //   Exception? exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

  //   return Results.Problem();
  // });

  // app.UseMiddleware<ErrorHandlingMiddleware>();
  // app.UseHttpsRedirection();
  app.MapControllers();
  app.Run();

}
