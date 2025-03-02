using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Apps.Api.Filters;
public class ErrorHandlingFilterAttributes : ExceptionFilterAttribute
{
  // This is invoked when an exception is thrown and was not hadled by the application
  public override void OnException(ExceptionContext context)
  {
    var problemDetails = new ProblemDetails
    {
      Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
      Title = "From Filters using problem Details: An Error Occured while processing your request",
      Status = (int)HttpStatusCode.InternalServerError,
      Detail = context.Exception.Message
    };
    context.Result = new ObjectResult(problemDetails);
    context.ExceptionHandled = true;
  }
}
