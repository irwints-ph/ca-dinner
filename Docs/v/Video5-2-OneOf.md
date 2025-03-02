
## OneOf
[Back][1]

Needed Package
```bash
dotnet add Apps\02-Apps.Application package oneOf
dotnet add Apps\01-Apps.Api package oneOf
```
[Extraction of to new method](#extraction-of-new-method)

1. Create [Duplicate Email][2] record
2. Use in [Auth Service Interface][4]
from 
```cs
  public AuthResult Register(...)
```
to
```cs
  public OneOf<AuthResult,DuplicateEmailError> Register(...)
```

3. Modify [Auth Service implementaion][5] to reflect changes
from
```cs
  public AuthResult Register(string Username, string Password, string FirstName, string LastName, string Email)
  {

    // Check if user already exist
    if(_userRepository.GetByEmail(Email) is not null){
      throw new Exception ("User with the given email already exists");
    }
    ...
  }
```
To
```cs
  public OneOf<AuthResult,DuplicateEmailError> Register(string Username, string Password, string FirstName, string LastName, string Email)
  {

    // Check if user already exist
    if(_userRepository.GetByEmail(Email) is not null){
      return new DuplicateEmailError();
    }
    // Create User
    var userData = new User
    {
      Username = Username,
      FirstName = FirstName,
      LastName = LastName,
      Email = Email,
      Password = Password
    };

    //Persist Data / Save to DB
    _userRepository.Add(userData);

    // Create JWT Token
    var token = _jwtToken.GenerateToken(userData);

    return new AuthResult(
      userData,
      token
    );
  } //End Method Register
```
> Will basically return DuplicateEmailError or AuthResult

4. Modify [Auth Controller][6] to implement chages
>From:
```cs
    public IActionResult Register(RegisterRequest request)
    {
      var authResult = _authService.Register(
          request.FirstName,
          request.LastName,
          request.Email,
          request.Password);
          
      var resisterResponse = new AuthResponse(
          authResult.User.Id,
          authResult.User.FirstName,
          authResult.User.LastName,
          authResult.User.Email,
          authResult.Token);
      
      return Ok(resisterResponse);
    }
```
>To: Version 1
```cs
  public IActionResult Register(RegisterRequest request)
  {
    OneOf<AuthResult,DuplicateEmailError> regResult = _authService.Register(request.Username, request.Password, request.FirstName, request.LastName, request.Email);
    
      if(regResult.IsT0){
        var authResult = regResult.AsT0;
        //Map to Contract Response
        var resisterResponse = new AuthResponse(
            authResult.User.Id,
            authResult.User.Username,
            authResult.User.FirstName,
            authResult.User.LastName,
            authResult.User.Email,
            authResult.Token      
        );
        return Ok(resisterResponse);
      }
      return Problem(statusCode: StatusCodes.Status409Conflict, title: "Register Controller: Email already exists.");    
  }
```
>To: Version 2
```cs
  public IActionResult Register(RegisterRequest request)
  {
    OneOf<AuthResult,DuplicateEmailError> regResult = _authService.Register(request.Username, request.Password, request.FirstName, request.LastName, request.Email);
    return regResult.Match(
      authResult => Ok(MapAuthResult(authResult)),
      _ => Problem(statusCode: StatusCodes.Status409Conflict, title: "Register Controller Match: Email already exists.")
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
> regResult.Match will return Ok if Good and _ if not

>To: Version 3

- Create [Error Interface][7]

- Modify [Auth Service Interface][4] to use the Error interface
```cs
  OneOf<AuthenticationResult,IError> Register(...)
```
- Modify [Auth Service implementaion][5] to reflect changes
- Modify [DuplicateEmailError][2]
from
```cs
public record struct DuplicateEmailError();
```
to
```cs
public record struct DuplicateEmailError : IError
{
    public readonly HttpStatusCode StatusCode => HttpStatusCode.Conflict;

    public readonly string ErrorMessage => "DuplicateEmailError Class: Email already used.";
}
```

- Modify [Auth Controller][6] to implement chages
```cs
  public IActionResult Register(RegisterRequest request)
  {
    OneOf<AuthResult,IError> regResult = _authService.Register(request.Username, request.Password, request.FirstName, request.LastName, request.Email);
    return regResult.Match(
      authResult => Ok(MapAuthResult(authResult)),
      _ => Problem(statusCode: StatusCodes.Status409Conflict, title: "Register Controller Match: Email already exists.")
    );

  }
```

#### Extraction of New Method
1. Highlight  code to be moved to method
2. Right Click then choose refactor
3. Choose extract method
4. Highlight Method name (Normally NewMetho)
5. right click then Rename Symbol (F2)
6. Name to the Method name needed

[Top][0] | [Back to main Flow Control][1]

[0]:#oneof
[1]:Video5-0.md
[2]:../../Apps/02-Apps.Application/Common/Errors/DuplicateEmailError.cs

[4]:../../Apps/02-Apps.Application/Services/Auth/IAuthService.cs
[5]:../../Apps/02-Apps.Application/Services/Auth/AuthService.cs
[6]:../../Apps/01-Apps.Api/Controllers/AuthController.cs
[7]:../../Apps/02-Apps.Application/Common/Errors/IError.cs