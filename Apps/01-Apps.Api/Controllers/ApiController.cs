
using Apps.Api.Common.Http;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Apps.Api.Controllers;
[ApiController]
public class ApiController : ControllerBase
{
  protected IActionResult Problem(List<Error> errors)
  {
    if(errors.Count is 0){
      return Problem();
    }
    if (errors.All(e => e.Type == ErrorType.Validation))
    {
        return ValidationProblems(errors);
    }

    HttpContext.Items[HttpContextItemKeys.Errors] = errors;
    return GenericProblem(errors[0]);
  }

  private ActionResult GenericProblem(Error error)
  {
    var statusCode = error.Type switch
    {
      ErrorType.Conflict => StatusCodes.Status409Conflict,
      ErrorType.Validation => StatusCodes.Status400BadRequest,
      ErrorType.NotFound => StatusCodes.Status404NotFound,
      _ => StatusCodes.Status500InternalServerError
    };

    return Problem(statusCode: statusCode, title: error.Description);
  }

  private ActionResult ValidationProblems(List<Error> errors)
  {
    var modelStateDictionary = new ModelStateDictionary();

    foreach (var e in errors)
    {
      modelStateDictionary.AddModelError(
        e.Code,
        e.Description
      );
    }
    return ValidationProblem(modelStateDictionary);
  }
}