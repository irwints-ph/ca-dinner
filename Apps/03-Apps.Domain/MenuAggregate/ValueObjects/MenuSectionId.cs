using Apps.Domain.Common.Models;

namespace Apps.Domain.MenuAggregate.ValueObjects;

public sealed class MenuSectionId : AggregateRootId<Guid>
{
  public override Guid Value { get; protected set; }
  private MenuSectionId(Guid value)
  {
    Value = value;
  }
  public static MenuSectionId CreateUniqe()
  {
    return new (Guid.NewGuid());
  }

  public static MenuSectionId Create(Guid value)
  {
    return new MenuSectionId(value);
  }

  public override IEnumerable<object> GetEqualityComponents()
  {
      yield return Value;
  }
  #pragma warning disable CS8618
  private MenuSectionId(){}
  #pragma warning restore CS8618

}