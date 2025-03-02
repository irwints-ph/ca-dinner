namespace Apps.Domain.Common.Models;

public abstract class AggregateRoot<TId> : Entity<TId>
  where TId : notnull
{
  protected AggregateRoot(TId id) : base(id){}


#pragma warning disable CS8604 // Possible null reference argument.
    protected AggregateRoot() : base(default(TId)){}
#pragma warning restore CS8604 // Possible null reference argument.


}