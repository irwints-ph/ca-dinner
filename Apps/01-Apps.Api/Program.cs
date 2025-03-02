using Apps.Application;
using Apps.Infrastructure;
using Apps.Api;

var builder = WebApplication.CreateBuilder(args);
{
  builder.Services
    .AddPresentation()
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    ;
}

var app = builder.Build();
{
  app.UseExceptionHandler("/error");
  // app.UseHttpsRedirection();  
  app.MapControllers();
  app.Run();

}
