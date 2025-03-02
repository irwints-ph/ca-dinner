using System.Security.Claims;
using Apps.Application.Common.Interfaces.Auth;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Apps.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Options;
using Apps.Domain.Entities;

namespace Apps.Infrastructure.Auth;
public class JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions) : IJwtTokenGenerator
{
  private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public string GenerateToken(User user)
    {
      var signingCredentials = new SigningCredentials(
          new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
          SecurityAlgorithms.HmacSha256);
        var claims = new []{
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
        new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        
      };
      var SecurityToken = new JwtSecurityToken(
        issuer: _jwtSettings.Issuer,
        audience: _jwtSettings.Audience,
        expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
        claims: claims,
        signingCredentials: signingCredentials
      );

      return new JwtSecurityTokenHandler().WriteToken(SecurityToken);
    }
}