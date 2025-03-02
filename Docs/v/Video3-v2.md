# using user class
[Back][1]

[AuthResults][2]
From
```cs
public record AuthResultLine(
  Guid Id,
  string Username,
  string FirstName,
  string LastName,
  string Email,
  string Token
);
```
To
```cs
public record AuthResultLine(
  User User,
  string Token
);
```
[Auth Service][3]
From
```cs
    // Create JWT Token
    var token = _jwtToken.GenerateToken(
      userData.Id,
      FirstName,
      LastName
    );

    return new AuthResult(
        userData.Id,
        userData.Username,
        userData.FirstName,
        userData.LastName,
        userData.Email,
        token
    );

```
To
```cs
    // Create JWT Token
    var token = _jwtToken.GenerateToken(userData);

    return new AuthResult(
        userData,
        token
    );
```
[Auth Controller][4]
From
```cs
      Line: new AuthResponseLine(
        loginResult.Id,
        loginResult.Username,
        loginResult.FirstName,
        loginResult.LastName,
        loginResult.Email,
        loginResult.Token
      )

```
To
```cs
      Line: new AuthResponseLine(
        loginResult.User.Id,
        loginResult.User.Username,
        loginResult.User.FirstName,
        loginResult.User.LastName,
        loginResult.User.Email,
        loginResult.Token
      )
```
[Token Generator Interface][5]
From
```cs
string GenerateToken(Guid Id, string FirstName, string LastName);
```
To
```cs
string GenerateToken(User user);
```

[Token Generator Implementation][6]
From
```cs
    public string GenerateToken(Guid Id, string FirstName, string LastName)
    {
      var signingCredentials = new SigningCredentials(
          new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
          SecurityAlgorithms.HmacSha256);
        var claims = new []{
        new Claim(JwtRegisteredClaimNames.Sub, Id.ToString()),
        new Claim(JwtRegisteredClaimNames.GivenName, FirstName),
        new Claim(JwtRegisteredClaimNames.FamilyName, LastName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),        
      };
    ...
    }
```
To
```cs
    public string GenerateToken(User user)
    {
      var signingCredentials = new SigningCredentials(
          new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
          SecurityAlgorithms.HmacSha256);
        var claims = new []{
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
        new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),        
      };
    ...
    }
```
[Auth Controller][7]
From
```cs
```
To
```cs
```

[Top][0] | [Back to main][1]

[0]:#using-user-class
[1]:../../readme.md
[2]:../../Apps/02-Apps.Application/Services/Auth/AuthResult.cs
[3]:../../Apps/02-Apps.Application/Services/Auth/AuthService.cs
[4]:../../Apps/01-Apps.Api/Controllers/AuthController.cs
[5]:../../Apps/02-Apps.Application/Common/Interfaces/Auth/IJwtTokenGenerator.cs
[6]:../../Apps/01-Apps.Infrastructure/Auth/JwtTokenGenerator.cs
