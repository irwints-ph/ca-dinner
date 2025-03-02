using ErrorOr;

namespace Apps.Application.Services.Auth;

public interface IAuthService
{
  ErrorOr<AuthResult> Login(
      string Username,
      string Password);
  ErrorOr<AuthResult> Register(
      string Username,
      string Password,
      string FirstName,
      string LastName,
      string Email);
}