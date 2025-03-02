using Apps.Application.Common.Errors;
using Apps.Application.Common.Interfaces.Auth;
using Apps.Application.Common.Interfaces.Persistence;
using Apps.Domain.Entities;
using FluentResults;

namespace Apps.Application.Services.Auth;

public class AuthService(
    IJwtTokenGenerator jwtToken,
    IUserRepository userRepository) : IAuthService
{
  private readonly IJwtTokenGenerator _jwtToken = jwtToken;
  private readonly IUserRepository _userRepository = userRepository;
  public AuthResult Login(string Username, string Password)
  {
    if(_userRepository.GetUser(Username) is not User loginData)
    {
      throw new Exception("User not found");
    }
    if(loginData.Password != Password)
    {
      throw new Exception("User incorect");
    }
    var token = _jwtToken.GenerateToken(loginData);

    return new AuthResult(
        loginData,
        token
    );
  } //End Method Login

  public Result<AuthResult> Register(
      string Username,
      string Password,
      string FirstName,
      string LastName,
      string Email)
  {

    // Check if user already exist
    if(_userRepository.GetByEmail(Email) is not null){
      return Result.Fail<AuthResult>(new DuplicateEmailError());
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
}