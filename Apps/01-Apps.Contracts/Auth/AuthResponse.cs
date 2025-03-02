namespace Apps.Contracts.Auth;
public record AuthResponse
(
  Guid Id,
  string Username,
  string FirstName,
  string LastName,
  string Email,
  string Token
);
