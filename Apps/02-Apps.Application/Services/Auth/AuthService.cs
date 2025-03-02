using Apps.Application.Common.Interfaces.Auth;
using Apps.Application.Common.Interfaces.Persistence;
using Apps.Domain.Common.Errors;
using Apps.Domain.Entities;
using ErrorOr;
namespace Apps.Application.Services.Auth;

public class AuthService(
    IJwtTokenGenerator jwtToken,
    IUserRepository userRepository) : IAuthService
{
  private readonly IJwtTokenGenerator _jwtToken = jwtToken;
  private readonly IUserRepository _userRepository = userRepository;
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

  public ErrorOr<AuthResult> Register(string Username, string Password, string FirstName, string LastName, string Email)
  {

    // Check if user already exist
    if(_userRepository.GetByEmail(Email) is not null){
      return Errors.User.DuplicateEmail;
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