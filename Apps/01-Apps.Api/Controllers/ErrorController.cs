using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Apps.Api.Controllers;

public class ErrorController : ControllerBase
{
  [Route("/error")]
    public IActionResult Error()
    {
      //Get the error from application
      Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

      return Problem(title: exception?.Message);
    }
}
