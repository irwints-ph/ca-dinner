using Apps.Domain.Entities;

namespace Apps.Application.Authentication.Common;
public record AuthResult
(
  User User,
  string Token
);
