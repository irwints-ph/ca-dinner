namespace Apps.Application.Services.Auth;
public record AuthResult
(
  Guid Id,
  string Username,
  string FirstName,
  string LastName,
  string Email,
  string Token
);
