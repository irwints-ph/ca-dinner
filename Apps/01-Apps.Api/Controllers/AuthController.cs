

using Apps.Application.Services.Auth;
using Apps.Contracts.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Apps.Api.Controllers;

[ApiController]
[Route("auth")]
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
        authResult.Id,
        authResult.Username,
        authResult.FirstName,
        authResult.LastName,
        authResult.Email,
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
        loginResult.Id,
        loginResult.Username,
        loginResult.FirstName,
        loginResult.LastName,
        loginResult.Email,
        loginResult.Token
    );
    return Ok(loginResponse);
  }
}