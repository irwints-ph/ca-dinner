# Model Validation
[Back][1]

### Creating the validation

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
    services.AddScoped<IPipelineBehavior<RegisterCommand,ErrorOr<AuthResult>>
      ,ValidateRegisterCommandBehavior>();
    
    // //Individual
    // services.AddScoped<IValidator<RegisterCommand>,RegisterCommandValidator>();
    //Running

```

### Add Fluent Validation
[Back][1]
1. Add FluentValidation Package to Application Project

```bash
dotnet add Apps\02-Apps.Application\ package FluentValidation
dotnet add Apps\02-Apps.Application\ package FluentValidation.AspNetCore
```

2. Create [Register Command Validator][4]

```cs
using FluentValidation;

namespace Apps.Application.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
  public RegisterCommandValidator()
  {
    RuleFor(x => x.Username).NotEmpty();
    RuleFor(x => x.FirstName).NotEmpty();
    RuleFor(x => x.LastName).NotEmpty();
    RuleFor(x => x.Email).NotEmpty();
    RuleFor(x => x.Password).NotEmpty();
  }
}
```

3. Register validation behavior to [DI of Application Project][3]

```cs
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
```

4. Use Fluent Validator on [Validation Behavior][2]
```cs
public class ValidateRegisterCommandBehavior(IValidator<RegisterCommand> validator) : IPipelineBehavior<RegisterCommand, ErrorOr<AuthResult>>
{
  private readonly IValidator<RegisterCommand> _validator = validator;

    public async Task<ErrorOr<AuthResult>> Handle(
      RegisterCommand request,
      RequestHandlerDelegate<ErrorOr<AuthResult>> next,
      CancellationToken cancellationToken)
  {
    var validationResult = _validator.ValidateAsync(request, cancellationToken);
    //if (validationResult.IsValid){
    if (validationResult.Result.IsValid){
      return await next();
    }

    // var errors = validationResult.Errors
    //   .Select(validationFailures => Errors.Validation(
    //     validationFailures.PropertyName,
    //     validationFailures.ErrorMessage
    //   ))
    //   .ToList();

    //Select(...).ToList() is equalt to ConvertAll(...)
    var errors = validationResult.Result.Errors
      .ConvertAll(validationFailures => Error.Validation(
        validationFailures.PropertyName,
        validationFailures.ErrorMessage
      ))
      ;

    return errors;
  }
}
```

## Convert To a Generic Validator
[Back][1]
1. Modify [Validation Behavior][2] to be generic
```cs
namespace Apps.Application.Common.Behaviors;
public class ValidateBehavior<TRequest, TResponse> : 
  IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
  private readonly IValidator<TRequest>? _validator;

    public ValidateBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

  public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
  {
    if(_validator is null)  return await next();

    var validationResult = _validator.ValidateAsync(request, cancellationToken);
    if (validationResult.Result.IsValid){
      return await next();
    }

    //Select(...).ToList() is equalt to ConvertAll(...)
    var errors = validationResult.Result.Errors
      .ConvertAll(validationFailures => Error.Validation(
        validationFailures.PropertyName,
        validationFailures.ErrorMessage
      ))
      ;

    return (dynamic)errors;
  } //End Handle Method
}
```
> dynamic key word should be avoided at all cost, but in this code
> we are sure that this will always return list of errors
> If not, this will be handled by the global error handling
> for us to investigate

2. Modify Registration of Validation Behavior in the [DI Of Application Project][3]
```cs
    services.AddScoped(
        typeof(IPipelineBehavior<,>),
        typeof(ValidateBehavior<,>));
```

## Fix Title of error response
[Back][1]
1. Modify Problem Module in [Base Controller][5] to return validation error if exist
```cs
  protected IActionResult Problem(List<Error> errors)
  {
    if(errors.All(e => e.Type == ErrorType.Validation))
    {
      var modelStateDictionary = new ModelStateDictionary();

      foreach(var e in errors)
      {
        modelStateDictionary.AddModelError(
          e.Code, 
          e.Description
        );
      }
      return ValidationProblem(modelStateDictionary);
    }

    HttpContext.Items[HttpContextItemKeys.Errors] = errors;    
    ...
  }
```
> Orignal Value is in #2 Above


## Base Controller Clean up

1. Modify [Base Controller][5] to extract code to methods if posible
```cs
namespace Apps.Api.Controllers;
[ApiController]
public class ApiController : ControllerBase
{
  protected IActionResult Problem(List<Error> errors)
  {
    if(errors.Count is 0){
      return Problem();
    }
    if (errors.All(e => e.Type == ErrorType.Validation))
    {
        return ValidationProblems(errors);
    }

    HttpContext.Items[HttpContextItemKeys.Errors] = errors;
    return GenericProblem(errors[0]);
  }

  private ActionResult GenericProblem(Error error)
  {
    var statusCode = error.Type switch
    {
      ErrorType.Conflict => StatusCodes.Status409Conflict,
      ErrorType.Validation => StatusCodes.Status400BadRequest,
      ErrorType.NotFound => StatusCodes.Status404NotFound,
      _ => StatusCodes.Status500InternalServerError
    };

    return Problem(statusCode: statusCode, title: error.Description);
  }

  private ActionResult ValidationProblems(List<Error> errors)
  {
    var modelStateDictionary = new ModelStateDictionary();

    foreach (var e in errors)
    {
      modelStateDictionary.AddModelError(
        e.Code,
        e.Description
      );
    }
    return ValidationProblem(modelStateDictionary);
  }
}
```

### Using the Generic validator

1. Create [Login Validator[]6]
```cs
using FluentValidation;

namespace Apps.Application.Authentication.Queries.Login;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
  public LoginQueryValidator()
  {
    RuleFor(x => x.Username).NotEmpty();
    RuleFor(x => x.Password).NotEmpty();
  }
}
```

## TODO
1. Validation for excess parameters

[Top][0] | [Back to main][1]

[0]:#validation-behavior
[1]:../../readme.md
[2]:../../Apps/02-Apps.Application/Common/Behaviors/ValidationBehavior.cs
[3]:../../Apps/02-Apps.Application/DependencyInjection.cs
[4]:../../Apps/02-Apps.Application/Authentication/Commands/Register/RegisterCommandValidator.cs
[5]:../../Apps/01-Apps.Api/Controllers/ApiController.cs
[6]:../../Apps/02-Apps.Application/Authentication/Queries/Login/LoginQueryValidator.cs