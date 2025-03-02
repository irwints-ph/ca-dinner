using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Apps.Application.Authentication.Commands.Register;
using ErrorOr;
using Apps.Application.Authentication.Common;
using Apps.Application.Common.Behaviors;
using FluentValidation;
using Microsoft.AspNetCore.WebSockets;
using System.Reflection;

namespace Apps.Application;

public static class DependencyInjection
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    services.AddMediatR(typeof(DependencyInjection).Assembly);

    //All RegisterCommand Validation
    services.AddScoped(
        typeof(IPipelineBehavior<,>),
        typeof(ValidateBehavior<,>));
    
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    return services;

  }
}