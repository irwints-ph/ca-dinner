using Apps.Domain.Common.Models;

namespace Apps.Domain.MenuAggregate.ValueObjects;

public sealed class MenuItemId : AggregateRootId<Guid>
{
  public override Guid Value { get; protected set; }
  private MenuItemId(Guid value)
  {
    Value = value;
  }
  public static MenuItemId CreateUniqe()
  {
    return new (Guid.NewGuid());
  }
  public static MenuItemId Create(Guid value)
  {
    return new MenuItemId(value);
  }

  public override IEnumerable<object> GetEqualityComponents()
  {
      yield return Value;
  }
  #pragma warning disable CS8618
  private MenuItemId(){}
  #pragma warning restore CS8618

}