namespace Apps.Contracts.Auth;
public record RegisterRequest
(
  string Username,
  string Password,
  string FirstName,
  string LastName,
  string Email
);
