using Apps.Application;
using Apps.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Apps.Api.Common.Errors;

var builder = WebApplication.CreateBuilder(args);
{
  builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    ;
    builder.Services.AddControllers();
    
    builder.Services.AddSingleton<ProblemDetailsFactory,AppsProblemDetailsFactory>();
}


var app = builder.Build();
{
  app.UseExceptionHandler("/error");
  // app.UseHttpsRedirection();
  app.MapControllers();
  app.Run();

}
