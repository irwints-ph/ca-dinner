using Apps.Application.Common.Interfaces.Auth;
using Apps.Application.Common.Interfaces.Services;
using Apps.Infrastructure.Auth;
using Apps.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Apps.Application.Common.Interfaces.Persistence;
using Apps.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Text;

namespace Apps.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
  {
    services.AddAuth(configuration);
    services.AddSingleton<IDateTimeProvider,DateTimeProvider>();
    services.AddScoped<IUserRepository,UserRepository>();
    return services;
  }
  public static IServiceCollection AddAuth(this IServiceCollection services, ConfigurationManager configuration)
  {
    JwtSettings jwtSettings = new ();
    configuration.Bind(JwtSettings.SectionName, jwtSettings);
    services.AddSingleton(Options.Create(jwtSettings));
    
    services.AddSingleton<IJwtTokenGenerator,JwtTokenGenerator>();

    services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(jwtSettings.Secret))        
      });
    return services;
  }
}