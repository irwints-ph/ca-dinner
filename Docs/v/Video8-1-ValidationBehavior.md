# Validation Behavior ***[Refernce](../BuberBreakfast/README.md)***
[Back][1]

1 Create [Validation Behavior][2]
```cs
using Apps.Application.Authentication.Commands.Register;
using Apps.Application.Authentication.Common;
using ErrorOr;
using MediatR;

namespace Apps.Application.Common.Behaviors;
public class ValidateRegisterCommandBehavior : IPipelineBehavior<RegisterCommand, ErrorOr<AuthResult>>
{
  public async Task<ErrorOr<AuthResult>> Handle(
      RegisterCommand request,
      RequestHandlerDelegate<ErrorOr<AuthResult>> next,
      CancellationToken cancellationToken)
  {
    //Executed before next handler in the pipe line    
    var result = await next();
    //Executed after handler run

    return result;
  }
}
```
2. Register validation behavior to [DI of Application Project][3]
```cs
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    services.AddMediatR(typeof(DependencyInjection).Assembly);
    services.AddScoped<IPipelineBehavior<RegisterCommand,ErrorOr<AuthResult>>
      ,ValidateRegisterCommandBehavior>();
    return services;
  }
```

[Top][0] | [Back to main][1]

[0]:#validation-behavior
[1]:../../readme.md
[2]:../../Apps/02-Apps.Application/Common/Behaviors/ValidationBehavior.cs
[3]:../../Apps/02-Apps.Application/DependencyInjection.cs