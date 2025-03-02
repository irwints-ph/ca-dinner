
using Apps.Api.Common.Http;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Apps.Api.Controllers;
[ApiController]
public class ApiController : ControllerBase
{
  //When set to public, this will require route
  //protected means that this is an internal class method
  protected IActionResult Problem(List<Error> errors)
  {
    HttpContext.Items[HttpContextItemKeys.Errors] = errors;

    var firstError = errors[0];
    var statusCode = firstError.Type switch {
      ErrorType.Conflict => StatusCodes.Status409Conflict,
      ErrorType.Validation => StatusCodes.Status400BadRequest,
      ErrorType.NotFound => StatusCodes.Status404NotFound,
      _ => StatusCodes.Status500InternalServerError
    };

    return Problem(statusCode: statusCode, title: firstError.Description);
  }
}