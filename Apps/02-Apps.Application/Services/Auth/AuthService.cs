namespace Apps.Application.Services.Auth;

public class AuthService : IAuthService
{
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

  public AuthResult Register(string Username, string Password, string FirstName, string LastName, string Email)
  {
    return new AuthResult(
        Guid.NewGuid(),
        Username,
        FirstName,
        LastName,
        Email,
        "Token"
    );
  }
}