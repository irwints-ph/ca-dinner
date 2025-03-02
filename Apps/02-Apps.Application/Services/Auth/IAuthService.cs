namespace Apps.Application.Services.Auth;
using FluentResults;

public interface IAuthService
{
  AuthResult Login(
      string Username,
      string Password);
  Result<AuthResult> Register(
      string Username,
      string Password,
      string FirstName,
      string LastName,
      string Email);
}