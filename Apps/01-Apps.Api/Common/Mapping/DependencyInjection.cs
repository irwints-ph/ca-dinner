using System.Reflection;
using Mapster;
using MapsterMapper;

namespace Apps.Api.Common.Mapping;

public static class DependencyInjection
{
  public static IServiceCollection AppMappings(this IServiceCollection services)
  {
    var config = TypeAdapterConfig.GlobalSettings;
    config.Scan(Assembly.GetExecutingAssembly());

    services.AddSingleton(config);
    services.AddScoped<IMapper, ServiceMapper>();
    
    return services;
  }
}