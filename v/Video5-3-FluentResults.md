## Fluent Results
[Back][1]
```bash
dotnet add Apps\02-Apps.Application package fluentresults
```

1. Modify [Auth Service Interface][4]
from 
```cs
  AuthResult Register(...)
```
to
```cs
  using FluentResults;
...

  Result<AuthResult> Register(...)
```
2. Modify [Auth Service implementaion][5] to reflect changes
```cs
  using FluentResults;
...

  Result<AuthResult> Register(...)
```
3. Implement IError from FluentResults. Create [Duplicate Error Class][7]
```cs
```

4. Modify [Auth Service implementaion][5] to reflect changes
>From:
```cs
  throw new Exception ("User with the given email already exists");
```
>To: Version 1
```cs
  return Result.Fail<AuthResult>(new DuplicateEmailError());
```
>To: Version 2
```cs
  return Result.Fail<AuthResult>(new [] { new DuplicateEmailError() });
```

5. Modify [Auth Controller][6] to implement chages
```cs
  public IActionResult Register(RegisterRequest request)
  {
    Result<AuthResult> authResult = _authService.Register(
        request.Username,
        request.Password,
        request.FirstName,
        request.LastName,
        request.Email
        );
    //Success
    if(authResult.IsSuccess){
      return Ok(MapAuthResult(authResult.Value));
    }
    //Error
    var firstResult = authResult.Errors[0];
    if(firstResult is DuplicateEmailError){
      return Problem(statusCode: StatusCodes.Status409Conflict, title: "FluentResult controller: Email already used");
    }
    return Problem();    
  }
```

[Top][0] | [Back to main Flow Control][1]

[0]:#fluent-results
[1]:Video5-0.md
[2]:../../Apps/02-Apps.Application/Common/Errors/DuplicateEmailError.cs

[4]:../../Apps/02-Apps.Application/Services/Auth/IAuthService.cs
[5]:../../Apps/02-Apps.Application/Services/Auth/AuthService.cs
[6]:../../Apps/01-Apps.Api/Controllers/AuthController.cs
[7]:../../Apps/02-Apps.Application/Common/Errors/DuplicateEmailError.cs