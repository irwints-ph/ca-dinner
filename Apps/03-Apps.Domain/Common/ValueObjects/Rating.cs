using Apps.Domain.Common.Models;

namespace Apps.Domain.Host.ValueObjects;

public sealed class Rating : AggregateRootId<int>
{
  public override int Value { get; protected set; }
  private Rating(int value)
  {
    Value = value;
  }
  public override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}