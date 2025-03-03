using Apps.Domain.Common.Models;

namespace Apps.Domain.HostAggregate.ValueObjects;

public sealed class HostId : AggregateRootId<string>
{
  public override string Value { get; protected set; }
  private HostId(string value)
  {
    Value = value;
  }
  public static HostId Create(string hostId)
  {
    return new HostId(hostId);
  }

  // public static HostId CreateUniqe()
  // {
  //   return new (Guid.NewGuid());
  // }
  public override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
  #pragma warning disable CS8618
  private HostId(){}
  #pragma warning restore CS8618

}