using ErrorOr;

namespace Apps.Domain.Common.Errors;
public static partial class Errors
{
  public static class User
  {
    public static Error DuplicateEmail => Error.Conflict(
        code: "User.DuplicateEmail",
        description: "Domain Error: Email already in use");

  }
}