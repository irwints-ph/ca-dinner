## ErrorOr
[Back][1]
```bash
dotnet add Apps\03-Apps.Domain package ErrorOr
```
1. Create [User Error][2] in the Domain Project
2. Modify [Auth Service Interface][4]
from 
```cs
  AuthResult Register(...)
```
to
```cs
  ErrorOr<AuthResult> Register(...)
```
3. Modify [Auth Service implementaion][5] to reflect changes
```cs
  public ErrorOr<AuthResult> Register(string Username, string Password, string FirstName, string LastName, string Email)
  {

    // Check if user already exist
    if(_userRepository.GetByEmail(Email) is not null){
      return Errors.User.DuplicateEmail;
    }
  ...
  }
```
4. Modify [Auth Controller][6] to implement chages
>From:
```cs
  public IActionResult Register(RegisterRequest request)
  {
    var authResult = _authService.Register(request.Username, request.Password, request.FirstName, request.LastName, request.Email);
    
    //Map to Contract Response
    var resisterResponse = new AuthResponse(authResult.User.Id, authResult.User.Username, authResult.User.FirstName, authResult.User.LastName, authResult.User.Email, authResult.Token);
    return Ok(resisterResponse);
  }
```
>To: Version 1
```cs
    public IActionResult Register(RegisterRequest request)
    {
        ErrorOr<AuthResult> authResult = _authenticationService.Register(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);
        return authResult.Match(
          a => Ok(MapAuthResult(a)),
          _ => Problem(
              statusCode: StatusCodes.Status409Conflict,
              title: "ErrorOr Controller: Email already used")
        );
    }

  private static AuthResponse MapAuthResult(AuthResult authResult)
  {
      //Map to Contract Response
      return new AuthResponse(
          authResult.User.Id,
          authResult.User.Username,
          authResult.User.FirstName,
          authResult.User.LastName,
          authResult.User.Email,
          authResult.Token
      );
  }
```
>To: Version 2
```cs
    public IActionResult Register(RegisterRequest request)
    {
        ErrorOr<AuthenticationResult> authResult = _authenticationService.Register(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);
        return authResult.MatchFirst(
          a => Ok(MapAuthResult(a)),
          firstError => Problem(
              statusCode: StatusCodes.Status409Conflict,
              title: firstError.Description)
        );
    }

```

### Update Login
1. Create [Authentication Error][7] in the Domain Project
2. Modify [Auth Service Interface][4]
from 
```cs
  AuthResult Login(...)
```
to
```cs
  ErrorOr<AuthResult> Login(...)
```
3. Modify [Auth Service implementaion][5] to reflect changes
```cs
  public ErrorOr<AuthResult> Login(string Username, string Password)
  {
    if(_userRepository.GetUser(Username) is not User loginData)
    {
      return Errors.Authentication.InvalidCredential;
    }
    if(loginData.Password != Password)
    {
      return Errors.Authentication.InvalidCredential;
    }
    var token = _jwtToken.GenerateToken(loginData);

    return new AuthResult(
        loginData,
        token
    );
  } //End Method Login
```
4. Modify [Auth Controller][6] to implement chages
From
```cs
  public IActionResult Login(LoginRequest request)
  {
    var loginResult = _authService.Login(
        request.Username,
        request.Password);
    
    //Map to Contract Response
    var loginResponse = new AuthResponse(
        loginResult.User.Id,
        loginResult.User.Username,
        loginResult.User.FirstName,
        loginResult.User.LastName,
        loginResult.User.Email,
        loginResult.Token
    );
    return Ok(loginResponse);
  }
```
```cs
  public IActionResult Login(LoginRequest request)
  {
    ErrorOr<AuthResult> loginResult = _authenticationService.Login(
        request.Email,
        request.Password);
    return loginResult.MatchFirst(
      a => Ok(MapAuthResult(a)),
      firstError => Problem(
          statusCode: StatusCodes.Status409Conflict,
          title: firstError.Description)
    );
  }
```

### Base Api
1. Create [Base Api Controller][8] For now to get the lost of errors
2. Modify [Auth Controller][6] to use the created base controller
From
```cs
[ApiController]
[Route("auth")]

public class AuthController(IAuthService authService) : ControllerBase
{
  ...
      return authResult.MatchFirst(
      a => Ok(MapAuthResult(a)),
      firstError => Problem(
          statusCode: StatusCodes.Status409Conflict,
          title: firstError.Description)
    );

}
```
To
```cs
[Route("auth")]

public class AuthController(IAuthService authService) : ApiController
{
  ...
    return authResult.Match(
      a => Ok(MapAuthResult(a)),
      e => Problem(e)
    );
}
```
> Note on the changing of Error Status Codes

### Add Custom Error

1. Modify [Base Api Controller][8] 
from
```cs
  protected IActionResult Problem(List<Error> errors)
  {
    var firstError = errors[0];
    var statusCode = firstError.Type switch {
      ErrorType.Conflict => StatusCodes.Status409Conflict,
      ErrorType.Validation => StatusCodes.Status400BadRequest,
      ErrorType.NotFound => StatusCodes.Status404NotFound,
      _ => StatusCodes.Status500InternalServerError
    };
    return Problem(statusCode: statusCode, title: firstError.Description);
  }
```
to
```cs
  protected IActionResult Problem(List<Error> errors)
  {
    HttpContext.Items["errors"] = errors;
    
    var firstError = errors[0];
    var statusCode = firstError.Type switch {
      ErrorType.Conflict => StatusCodes.Status409Conflict,
      ErrorType.Validation => StatusCodes.Status400BadRequest,
      ErrorType.NotFound => StatusCodes.Status404NotFound,
      _ => StatusCodes.Status500InternalServerError
    };

    return Problem(statusCode: statusCode, title: firstError.Description);
  }
```

2. Modify [Problem Factory][9]
```cs
        //Add On
        var errors = httpContext?.Items["errors"] as List<Error>;
        if(errors is not null)
        {
            problemDetails.Extensions.Add("errorCodes",errors.Select(e => e.Code));
        }

```

### Add Custom Error - replace string identifier to constant

1, Create [Constant Key file][10]
1. Modify [Base Api Controller][8] 
From
```cs
  protected IActionResult Problem(List<Error> errors)
  {
    HttpContext.Items["errors"] = errors;
    ...
  }
```
to
```cs
  protected IActionResult Problem(List<Error> errors)
  {
    HttpContext.Items[HttpContextItemKeys.Errors] = errors;
    ...
  }
```
2. Modify [Problem Factory][9]
```cs
  var errors = httpContext?.Items["errors"] as List<Error>;
```
to
```cs
var errors = httpContext?.Items[HttpContextItemKeys.Errors] as List<Error>;
```
### Overwrite Errors

1. Modify [Auth Controller][6] to add error handling
From
```cs
  public IActionResult Login(LoginRequest request)
  {
    ErrorOr<AuthResult> loginResult = _authService.Login(
        request.Username,
        request.Password);

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
  }
}```


[Top][0] | [Back to main Flow Control][1]

[0]:#erroror
[1]:Video5-0.md
[2]:../../Apps/03-Apps.Domain/Common/Errors/Errors.User.cs
[3]:../../Apps/01-Apps.Api/Program.cs
[4]:../../Apps/02-Apps.Application/Services/Auth/IAuthService.cs
[5]:../../Apps/02-Apps.Application/Services/Auth/AuthService.cs
[6]:../../Apps/01-Apps.Api/Controllers/AuthController.cs
[7]:../../Apps/03-Apps.Domain/Common/Errors/Errors.Authentication.cs
[8]:../../Apps/01-Apps.Api/Controllers/ApiController.cs
[9]:../../Apps/01-Apps.Api/Common/Errors/AppsProblemDetailsFactory.cs
[10]:../../Apps/01-Apps.Api/Common/Http/HttpContextItemKeys.cs