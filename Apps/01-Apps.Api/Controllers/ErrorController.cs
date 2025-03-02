using Apps.Application.Common.Errors;
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
      
      var (statusCode, Message) = exception switch
      {
        IServiceException serviceException => ((int)serviceException.StatusCode, serviceException.ErrorMessage),
        _ => (StatusCodes.Status500InternalServerError, "Internal Server Error: " + exception?.Message)
      };
      return Problem(statusCode: statusCode ,title: Message);
    }
}
