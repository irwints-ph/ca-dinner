using ErrorOr;
using FluentValidation;
using MediatR;

namespace Apps.Application.Common.Behaviors;
public class ValidateBehavior<TRequest, TResponse> : 
  IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
  private readonly IValidator<TRequest>? _validator;

    public ValidateBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

  public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
  {
    if(_validator is null)  return await next();

    var validationResult = _validator.ValidateAsync(request, cancellationToken);
    if (validationResult.Result.IsValid){
      return await next();
    }

    //Select(...).ToList() is equalt to ConvertAll(...)
    var errors = validationResult.Result.Errors
      .ConvertAll(validationFailures => Error.Validation(
        validationFailures.PropertyName,
        validationFailures.ErrorMessage
      ))
      ;

    return (dynamic)errors;
  } //End Handle Method

}