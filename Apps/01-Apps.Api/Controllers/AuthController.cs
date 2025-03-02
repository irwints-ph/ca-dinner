

// using Apps.Api.Filters;
using Apps.Application.Authentication.Commands.Register;
using Apps.Application.Authentication.Common;
using Apps.Application.Authentication.Queries.Login;
using Apps.Contracts.Auth;
using Apps.Domain.Common.Errors;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apps.Api.Controllers;

[Route("auth")]
[AllowAnonymous]
public class AuthController(ISender mediator, IMapper mapper) : ApiController
{
  private readonly ISender _mediator = mediator;

  private readonly IMapper _mapper = mapper;

  [HttpPost("register")]
  public async Task<IActionResult> Register(RegisterRequest request)
  {
    //Note the arrange ment on how this is sent
    var command = new RegisterCommand(
      request.Username,
      request.Password,
      request.FirstName,
      request.LastName,
      request.Email
      );

    ErrorOr<AuthResult> authResult = await _mediator.Send(command);

    return authResult.Match(
      a => Ok(_mapper.Map<AuthResult>(a)),
      e => Problem(e)
    );

  } //End Method Reigister

  [HttpPost("login")]
  public async Task<IActionResult> Login(LoginRequest request)
  {
    var query = new LoginQuery(
      request.Username,
      request.Password);

    ErrorOr<AuthResult> loginResult = await _mediator.Send(query);

    //Modify error codes
    if(loginResult.IsError && loginResult.FirstError == Errors.Authentication.InvalidCredential){
      return Problem(
          statusCode: StatusCodes.Status401Unauthorized,
          title: loginResult.FirstError.Description);
    }

    return loginResult.Match(
      a => Ok(_mapper.Map<AuthResult>(a)),
      e => Problem(e)
    );

  }//End Method Login

  private static AuthResponse MapAuthResult(AuthResult authResult)
  {
      //Map to Contract Response
      return new AuthResponse(
          authResult.User.Id,
          authResult.User.Username,
          authResult.User.FirstName,
          authResult.User.LastName,
          authResult.User.Email,
          authResult.Token
      );
  }

}