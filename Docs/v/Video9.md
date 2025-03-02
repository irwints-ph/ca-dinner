# Bearer Authentication
[Back][1]
1. Create a blank controller/[end point][2] to test authentication
```cs
using Microsoft.AspNetCore.Mvc;

namespace Apps.Api.Controllers;
[Route("[Controller]")]
public class DinnerController : ApiController
{
  [HttpGet]
  public IActionResult ListDinners()
  {
    return Ok(Array.Empty<string>()); //return Ok([]);
  }
}
```
2. Create [Request Tester for the end point][4]
```cs
GET {{host}}/dinner
```
3. run with break point as mentioned in Video
4. Add the UseAuthentication middleware in [Program.cs][30]
```cs
var app = builder.Build();
{
  ...
  app.UseAuthentication();
  ...
}
```
5. Add package Microsoft.AspNetCore.Authentication.JwtBearer to the Infrastructure Project
> for core 8 use version 8  --version 8.0.11
```bash
dotnet add .\Apps\01-Apps.Infrastructure package Microsoft.AspNetCore.Authentication.JwtBearer  --version 8.0.11
```

6. Configure Authentication(Jwt) in the [DI of Infrastructure Project][33]
From
```cs
namespace Apps.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(
      this IServiceCollection services,
      ConfigurationManager configuration)
  {
    services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
    services.AddSingleton<IJwtTokenGenerator,JwtTokenGenerator>();

    services.AddSingleton<IDateTimeProvider,DateTimeProvider>();
    services.AddScoped<IUserRepository,UserRepository>();
    return services;
  }
}
```
To
```cs
...
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
    //services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
    //Replace By Beg
    JwtSettings jwtSettings = new ();
    configuration.Bind(JwtSettings.SectionName, jwtSettings);
    services.AddSingleton(Options.Create(jwtSettings));
    //Replace By End
    
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
```
7. Test as per the Video
8. Register first then modify [Request Tester for the end point][4] to include the token
```cs
GET {{host}}/dinner
Authorization: Bearer {{token}}
```
this.HttpContext.User.Identity.IsAuthenticated

((System.Security.Claims.ClaimsIdentity)(new Microsoft.AspNetCore.Http.HttpContext.HttpContextDebugView(this.HttpContext).User).Identity).IsAuthenticated

### Adding Authorization
[Top][0] | [Back][1]

1. Add UseAuthorization middleware on [Program.cs][30]
```cs
var app = builder.Build();
{
  ...
  app.UseAuthentication();
  app.UseAuthorization();
  ...
}
```

2. Modify the [end point][2] to use Authorization

```cs
using Microsoft.AspNetCore.Mvc;

namespace Apps.Api.Controllers;
[Route("[Controller]")]
[Authorize]
public class DinnerController : ApiController
{
  ...
}
```

### Adding Authorization Global - to inheriting classes
[Top][0] | [Back][1]

1. Remove [Authorize] to the Test End Point 
1. Move the [Authorize] to the base controller
1. Place the [AllowAnonymous] to the controller that is expempted from Authorization like login and register

[Top][0] | [Back to main][1]

[0]:#bearer-authentication
[1]:../../readme.md#contents
[2]:../../Apps/01-Apps.Api/Controllers/DinnerController.cs
[3]:../../Apps/02-Apps.Application/DependencyInjection.cs
[30]:../../Apps/01-Apps.Api/Program.cs
[33]:../../Apps/01-Apps.Infrastructure/DependencyInjection.cs
[4]:../Request/Dinners/ListDinners.http
