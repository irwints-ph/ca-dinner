using Apps.Domain.Common.Models;

namespace Apps.Domain.Host.ValueObjects;

public sealed class Rating : ValueObject
{
  public int Value { get; }
  private Rating(int value)
  {
    Value = value;
  }
  // public static Rating CreateUniqe()
  // {
  //   return new (Guid.NewGuid());
  // }
  public override IEnumerable<object> GetEqualityComponents()
  {
      yield return Value;
  }
}