## Via Execption
[Back][1]

### Variantion 1
1. Remove all the global error handling in Video 4 
  a. in [Program.cs][3]

```csharp
  var builder = WebApplication.CreateBuilder(args);
  {
    builder.Services
      .AddInfrastructure(builder.Configuration)
      .AddApplication();
      builder.Services.AddControllers();      
      builder.Services.AddSingleton<ProblemDetailsFactory,AppsProblemDetailsFactory>();
  }

  var app = builder.Build();
  {
    app.UseExceptionHandler("/error");
    app.MapControllers();
    app.Run();
  }
```
  b. create [Duplicate Email Execption Class][2]
```cs
public class DuplicateEmailException : Exception, IServiceException
{}
```  
  c. Modify AuthService[4]
```cs  
    throw new Exception ("App Service: User with the given email already exists"); became => throw new DuplicateEmailException();
```    
  d. Use in [ErrorController][5] of the API project
```cs  
      var (statusCode, Message) = exception switch
      {
        DuplicateEmailException => (StatusCodes.Status409Conflict, "Email already exists."),
        _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
      };
      return Problem(statusCode: statusCode ,title: Message);
```
> This will show the error Email Already Exist if the type of execption is DuplicateEmailException; Otherwise the Error is "Internal Server Error"

> Alternatively you can make the otherwise statement like this to get the message thrown
```cs
_ => (StatusCodes.Status500InternalServerError, exception?.Message)
```

### Variation
  d.1 Create an [Exception Service Interface][6] in the Application Project

  d.2 Modify [Duplicate Email Execption Class][2] to use the interface
```cs
  public class DuplicateEmailException : Exception, IServiceException
  {
    public HttpStatusCode StatusCode => HttpStatusCode.Conflict;
    public string ErrorMessage => "DuplicateEmailException: Email Already Exists";
  }
```  
  d.3 Use in [ErrorController][5] of the API project
```cs  
  var (statusCode, Message) = exception switch
  {
    IServiceException serviceException => ((int)serviceException.StatusCode, serviceException.ErrorMessage),
    _ => (StatusCodes.Status500InternalServerError, "Internal Server Error: " + exception?.Message)
  };
  return Problem(statusCode: statusCode ,title: Message);
```

[Top][0] | [Back to main Flow Control][1]

[0]:#via-execption
[1]:Video5-0.md
[2]:../../Apps/02-Apps.Application/Common/Errors/DuplicateEmailException.cs
[3]:../../Apps/01-Apps.Api/Program.cs
[4]:../../Apps/02-Apps.Application/Services/Auth/AuthService.cs
[5]:../../Apps/01-Apps.Api/Controllers/ErrorController.cs
[6]:../../Apps/02-Apps.Application/Common/Errors/IServiceException.cs