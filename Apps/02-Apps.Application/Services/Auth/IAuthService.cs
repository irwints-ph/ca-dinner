namespace Apps.Application.Services.Auth;

public interface IAuthService
{
  AuthResult Login(
      string Username,
      string Password);
  AuthResult Register(
      string Username,
      string Password,
      string FirstName,
      string LastName,
      string Email);
}