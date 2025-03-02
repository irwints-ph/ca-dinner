using System.Net;

namespace Apps.Application.Common.Errors;
public class DuplicateEmailException : Exception, IServiceException
{
  public HttpStatusCode StatusCode => HttpStatusCode.Conflict;
  public string ErrorMessage => "DuplicateEmailException: Email Already Exists";
}
