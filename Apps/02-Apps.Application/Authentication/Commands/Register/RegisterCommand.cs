using Apps.Application.Authentication.Common;
using ErrorOr;
using MediatR;

namespace Apps.Application.Authentication.Commands.Register;
public record RegisterCommand(
  string Username,
  string Password,
  string FirstName,
  string LastName,
  string Email
  ) : IRequest<ErrorOr<AuthResult>>;