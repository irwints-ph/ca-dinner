using Apps.Domain.Common.Models;

namespace Apps.Domain.MenuReviewAggregate.ValueObjects;

public sealed class MenuReviewId : AggregateRootId<Guid>
{
  public override Guid Value { get; protected set; }
  private MenuReviewId(Guid value)
  {
    Value = value;
  }
  public static MenuReviewId CreateUniqe()
  {
    return new (Guid.NewGuid());
  }
  public override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
  #pragma warning disable CS8618
  private MenuReviewId(){}
  #pragma warning restore CS8618

}