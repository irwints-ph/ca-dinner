namespace Apps.Domain.Common.Models;

public abstract class AggregateRootId<TId> : ValueObject
{

  public abstract TId Value { get; protected set; }
  
#pragma warning disable CS8604 // Possible null reference argument.
    protected AggregateRootId() {}
#pragma warning restore CS8604 // Possible null reference argument.


}