using Apps.Application.Common.Interfaces.Auth;
namespace Apps.Application.Services.Auth;

public class AuthService(IJwtTokenGenerator jwtToken) : IAuthService
{
  private readonly IJwtTokenGenerator _jwtToken = jwtToken;

    public AuthResult Login(string Username, string Password)
  {
    return new AuthResult(
        Guid.NewGuid(),
        Username,
        "Fistname",
        "Lastname",
        "Email",
        "Token"
    );
  }

  public AuthResult Register(
      string Username,
      string Password,
      string FirstName,
      string LastName,
      string Email)
  {

    // Check if user already exist
    // Create User

    var userId = Guid.NewGuid();

    // Create JWT Token
    var token = _jwtToken.GenerateToken(
        userId,
        FirstName,
        LastName);

    return new AuthResult(
        userId,
        Username,
        FirstName,
        LastName,
        Email,
        token
    );
  }
}