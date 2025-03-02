using Apps.Application.Common.Errors;
using OneOf;

namespace Apps.Application.Services.Auth;

public interface IAuthService
{
  AuthResult Login(
      string Username,
      string Password);
  OneOf<AuthResult,IError> Register(
      string Username,
      string Password,
      string FirstName,
      string LastName,
      string Email);
}