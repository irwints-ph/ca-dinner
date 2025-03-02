using ErrorOr;

namespace Apps.Domain.Common.Errors;
public static partial class Errors
{
  public static class Authentication
  {
    public static Error InvalidCredential => Error.Validation(
        code: "Auth.InvalidCred",
        description: "Domain Error: Invalid Credentials");

  }
}