namespace Apps.Application.Common.Interfaces.Auth;
public interface IJwtTokenGenerator
{
  string GenerateToken(Guid Id, string FirstName, string LastName);
}
