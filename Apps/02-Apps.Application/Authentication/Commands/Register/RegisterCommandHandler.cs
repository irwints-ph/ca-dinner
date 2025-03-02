using Apps.Application.Authentication.Common;
using Apps.Application.Common.Interfaces.Auth;
using Apps.Application.Common.Interfaces.Persistence;
using Apps.Domain.Common.Errors;
using Apps.Domain.Entities;
using ErrorOr;
using MediatR;

namespace Apps.Application.Authentication.Commands.Register;
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthResult>>
{

  private readonly IJwtTokenGenerator _jwtTokenGenerator;
  private readonly IUserRepository _userRepository;
  public RegisterCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
  {
    _jwtTokenGenerator = jwtTokenGenerator;
    _userRepository = userRepository;
  }
  public async Task<ErrorOr<AuthResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
  {
    //0 Temporary to remove warning
    await Task.CompletedTask;

    // 1. Check if user does not exist
    if(_userRepository.GetByEmail(command.Email) is not null)
    {
      return Errors.User.DuplicateEmail;
    }

    // 2. Create User (Generate Unique ID ) and persist/save to DB
    var user = new User
    {
      Username = command.Username,
      FirstName = command.FirstName,
      LastName = command.LastName,
      Email = command.Email,
      Password = command.Password
    };

    // 2.1 Persist/Save User to DB
    _userRepository.Add(user);

    // 3. Create JWT Token
    var token = _jwtTokenGenerator.GenerateToken(user);

    return new AuthResult(
      user,
      token
    );
  }
}