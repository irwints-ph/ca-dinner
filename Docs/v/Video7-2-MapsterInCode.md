# Mapster In Code
[Back][1]
```bash
dotnet add Apps\01-Apps.Api\ package Mapster
dotnet add Apps\01-Apps.Api\ package Mapster.DependencyInjection
```


1. [Create Mapping Class][2] 
```cs
public class AuthenticationMappingConfig : IRegister
{
  public void Register(TypeAdapterConfig config)
  {
    //2 Mapping is to document mapping even though they have the same structure
    config.NewConfig<RegisterRequest, RegisterCommand>();
    config.NewConfig<LoginRequest, LoginQuery>();

    config.NewConfig<AuthResult,AuthResponse>()
      .Map(d => d.Token, s => s.Token)
      .Map(d => d, s => s.User);
  }
}
```
2. [Create DI for All Mapping][3]
```cs
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
```
3. Create DI for Presentation Project and register presentation services[4]
```cs
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
```
4. Register Presentation DI to [Program.cs][30] and remove those transfered to Presentation DI
```cs
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
```

5. Add Mapper to [Auth Controller][6]
From
```cs
public class AuthController(ISender mediator) : ApiController
{
  private readonly ISender _mediator = mediator;
  ...
}
```
To
```cs
using MapsterMapper;
  ...
public class AuthController(ISender mediator, IMapper mapper) : ApiController
{
  private readonly ISender _mediator = mediator;

  private readonly IMapper _mapper = mapper;
  ...
}
```

6. Use Mapper to map return of [Auth Controller][6]
From
```cs
  public async Task<IActionResult> Login(LoginRequest request)
  {
    var query = new LoginQuery(
      request.Username,
      request.Password);

    ErrorOr<AuthResult> loginResult = await _mediator.Send(query);

    //Modify error codes
    if(loginResult.IsError && loginResult.FirstError == Errors.Authentication.InvalidCredential){
      return Problem(
          statusCode: StatusCodes.Status401Unauthorized,
          title: loginResult.FirstError.Description);
    }

    return loginResult.Match(
      a => Ok(MapAuthResult(a)),
      e => Problem(e)
    );    
  }//End Method Login
```
To
```cs
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = _mapper.Map<LoginQuery>(request);
        ErrorOr<AuthenticationResult> authResult = await _mediator.Send(query);

        if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredential)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: authResult.FirstError.Description);
        }
        return loginResult.Match(
          a => Ok(_mapper.Map<AuthResult>(a)),
          e => Problem(e)
        );
    }
```
Create [Mapping Config][2]
Create [Dependency Injection for Mapping Config][3]
Create [Dependency Injection for Presentation Layer][4]
Add DI of Mapping Config and other Presentation Layer Dependecy
Clean Up Dependency Injection of [Program.cs][30]

[Top][0] | [Back to main][1]

[0]:#mapster-in-code
[1]:../../readme.md
[2]:../../Apps/01-Apps.Api/Common/Mapping/AuthenticationMappingConfig.cs
[3]:../../Apps/01-Apps.Api/Common/Mapping/DependencyInjection.cs
[30]:../../Apps/01-Apps.Api/Program.cs
[4]:../../Apps/01-Apps.Api/DependencyInjection.cs

[5]:../../Apps/02-Apps.Application/Services/Auth/AuthService.cs
[6]:../../Apps/01-Apps.Api/Controllers/AuthController.cs
[7]:../../Apps/03-Apps.Domain/Common/Errors/Errors.Authentication.cs
[8]:../../Apps/01-Apps.Api/Controllers/ApiController.cs
[9]:../../Apps/01-Apps.Api/Common/Errors/AppsProblemDetailsFactory.cs
[10]:../../Apps/01-Apps.Api/Common/Http/HttpContextItemKeys.cs