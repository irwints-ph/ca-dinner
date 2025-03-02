using Apps.Domain.Common.Models;

namespace Apps.Domain.DinnerAggregate.ValueObjects;

public sealed class DinnerId : ValueObject
{
  public Guid Value { get; private set; }
  private DinnerId(Guid value)
  {
    Value = value;
  }
  public static DinnerId CreateUniqe()
  {
    return new (Guid.NewGuid());
  }
  public override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
  #pragma warning disable CS8618
  private DinnerId(){}
  #pragma warning restore CS8618

}