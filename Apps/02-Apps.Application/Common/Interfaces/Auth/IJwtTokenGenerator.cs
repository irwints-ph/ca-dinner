using Apps.Domain.Entities;

namespace Apps.Application.Common.Interfaces.Auth;
public interface IJwtTokenGenerator
{
  string GenerateToken(User user);
}
