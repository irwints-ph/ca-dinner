using Apps.Api.Common.Errors;
using Apps.Api.Common.Mapping;
using Microsoft.AspNetCore.Mvc.Infrastructure;
namespace Apps.Api;

public static class DependencyInjection
{
  public static IServiceCollection AddPresentation(this IServiceCollection services)
  {
    services.AppMappings();
    services.AddControllers();
    
    services.AddSingleton<ProblemDetailsFactory,AppsProblemDetailsFactory>();

    return services;
  }
}