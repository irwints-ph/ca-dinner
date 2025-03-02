using Apps.Domain.Entities;

namespace Apps.Application.Services.Auth;
public record AuthResult
(
  User User,
  string Token
);
