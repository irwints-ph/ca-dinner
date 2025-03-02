using Apps.Application.Authentication.Common;
using Apps.Application.Common.Interfaces.Auth;
using Apps.Application.Common.Interfaces.Persistence;
using Apps.Domain.Common.Errors;
using Apps.Domain.Entities;
using ErrorOr;
using MediatR;

namespace Apps.Application.Authentication.Queries.Login;
public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthResult>>
{

  private readonly IJwtTokenGenerator _jwtTokenGenerator;
  private readonly IUserRepository _userRepository;
  public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
  {
    _jwtTokenGenerator = jwtTokenGenerator;
    _userRepository = userRepository;
  }
  
  public async Task<ErrorOr<AuthResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
  {
    //0 Temporary to remove warning
    await Task.CompletedTask;
    
    // 1. Check if user exist    
    // if(_userRepository.GetByEmail(query.Email) is not User user)
    // {
    //   //🛑 Danger: This is a security vulnerability. Do not expose this information to the client.
    //   return Errors.Authentication.InvalidCredential;
    // }
    if(_userRepository.GetUser(query.Username) is not User user)
    {
      return Errors.Authentication.InvalidCredential;
    }

    // 2. Check if password is correct
    if(user.Password != query.Password)
    { 
      return Errors.Authentication.InvalidCredential;
    }
    
    // 3. Create JWT Token
    var token = _jwtTokenGenerator.GenerateToken(user);

    return new AuthResult(
      user,
      token
    );
  }
}