using Apps.Application.Authentication.Commands.Register;
using Apps.Application.Authentication.Common;
using Apps.Application.Authentication.Queries.Login;
using Apps.Contracts.Auth;
using Mapster;

namespace Apps.Api.Common.Mapping;
public class AuthenticationMappingConfig : IRegister
{
  public void Register(TypeAdapterConfig config)
  {
    //2 Mapping is to document mapping even though they have the same structure
    config.NewConfig<RegisterRequest, RegisterCommand>();
    config.NewConfig<LoginRequest, LoginQuery>();

    config.NewConfig<AuthResult,AuthResponse>()
      .Map(d => d.Token, s => s.Token)
      .Map(d => d, s => s.User);
  }
}