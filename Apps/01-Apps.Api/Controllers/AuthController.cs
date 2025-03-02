

// using Apps.Api.Filters;
using Apps.Application.Services.Auth;
using Apps.Contracts.Auth;
using Apps.Domain.Common.Errors;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Apps.Api.Controllers;

[Route("auth")]

public class AuthController(IAuthService authService) : ApiController
{
  private readonly IAuthService _authService = authService;

  [HttpPost("register")]
  public IActionResult Register(RegisterRequest request)
  {
    ErrorOr<AuthResult> authResult = _authService.Register(request.Username, request.Password, request.FirstName, request.LastName, request.Email);
    
    //Map to Contract Response - Version 1
    // return authResult.Match(
    //       a => Ok(MapAuthResult(a)),
    //       _ => Problem(
    //           statusCode: StatusCodes.Status409Conflict,
    //           title: "ErrorOr Controller: Email already used")
    // );

    return authResult.Match(
      a => Ok(MapAuthResult(a)),
      e => Problem(e)
    );

  }

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
  [HttpPost("login")]
  public IActionResult Login(LoginRequest request)
  {
    // var loginResult = _authService.Login(
    //     request.Username,
    //     request.Password);

    ErrorOr<AuthResult> loginResult = _authService.Login(
        request.Username,
        request.Password);

    //Modify error codes
    if(loginResult.IsError && loginResult.FirstError == Errors.Authentication.InvalidCredential){
      return Problem(
          statusCode: StatusCodes.Status401Unauthorized,
          title: loginResult.FirstError.Description);
    }

    return loginResult.Match(
      a => Ok(MapAuthResult(a)),
      e => Problem(e)
    );    
  }
}