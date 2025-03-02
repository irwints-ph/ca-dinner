using Apps.Application.Authentication.Common;
using ErrorOr;
using MediatR;

namespace Apps.Application.Authentication.Queries.Login;
public record LoginQuery(
  string Username,
  string Password) : IRequest<ErrorOr<AuthResult>>;