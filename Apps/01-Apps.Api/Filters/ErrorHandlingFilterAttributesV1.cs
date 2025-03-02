using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Apps.Api.Filters;
public class ErrorHandlingFilterAttributesV1 : ExceptionFilterAttribute
{
    // This is invoked when an exception is thrown and was not hadled by the application
    public override void OnException(ExceptionContext context)
    {
      var exception = context.Exception;
      context.Result = new ObjectResult(new { error = "From Filters: An Error Occured while processing your request" })
      {
        StatusCode = 500
      };
      context.ExceptionHandled = true;
    }
}
