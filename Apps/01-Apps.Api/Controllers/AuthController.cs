

// using Apps.Api.Filters;
using Apps.Application.Services.Auth;
using Apps.Contracts.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Apps.Api.Controllers;

[ApiController]
[Route("auth")]
// [ErrorHandlingFilterAttributes]
public class AuthController(IAuthService authService) : ControllerBase
{
  private readonly IAuthService _authService = authService;

  [HttpPost("register")]
  public IActionResult Register(RegisterRequest request)
  {
    var authResult = _authService.Register(
        request.Username,
        request.Password,
        request.FirstName,
        request.LastName,
        request.Email
        );
    
    //Map to Contract Response
    var resisterResponse = new AuthResponse(
        authResult.User.Id,
        authResult.User.Username,
        authResult.User.FirstName,
        authResult.User.LastName,
        authResult.User.Email,
        authResult.Token      
    );
    return Ok(resisterResponse);
  }

  [HttpPost("login")]
  public IActionResult Login(LoginRequest request)
  {
    var loginResult = _authService.Login(
        request.Username,
        request.Password);
    
    //Map to Contract Response
    var loginResponse = new AuthResponse(
        loginResult.User.Id,
        loginResult.User.Username,
        loginResult.User.FirstName,
        loginResult.User.LastName,
        loginResult.User.Email,
        loginResult.Token
    );
    return Ok(loginResponse);
  }
}