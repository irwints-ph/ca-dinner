using Apps.Application.Common.Interfaces.Auth;
using Apps.Application.Common.Interfaces.Services;
using Apps.Infrastructure.Auth;
using Apps.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Apps.Application.Common.Interfaces.Persistence;
using Apps.Infrastructure.Persistence;

namespace Apps.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(
      this IServiceCollection services,
      ConfigurationManager configuration)
  {
    services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
    services.AddSingleton<IDateTimeProvider,DateTimeProvider>();
    services.AddSingleton<IJwtTokenGenerator,JwtTokenGenerator>();
    services.AddScoped<IUserRepository,UserRepository>();
    return services;
  }
}