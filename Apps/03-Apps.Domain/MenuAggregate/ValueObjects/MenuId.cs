using Apps.Domain.Common.Models;

namespace Apps.Domain.MenuAggregate.ValueObjects;

public sealed class MenuId : AggregateRootId<Guid>
{
  public override Guid Value { get; protected set; }
  private MenuId(Guid value)
  {
    Value = value;
  }
  public static MenuId CreateUniqe()
  {
    return new (Guid.NewGuid());
  }
  public static MenuId Create(Guid value)
  {
    return new MenuId(value);
  }

  public override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
  #pragma warning disable CS8618
  private MenuId(){}
  #pragma warning restore CS8618

}