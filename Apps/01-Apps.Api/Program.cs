using Apps.Application;
using Apps.Application.Services.Auth;
using Apps.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
  builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    ;
  builder.Services.AddControllers();
}


var app = builder.Build();
{
  // app.UseHttpsRedirection();
  app.MapControllers();
  app.Run();

}
