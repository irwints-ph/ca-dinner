using System.Net;

namespace Apps.Application.Common.Errors;

public interface IServiceException
{
  public HttpStatusCode StatusCode { get; }
  public string ErrorMessage { get; }
}
