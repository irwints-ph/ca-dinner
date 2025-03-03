namespace Apps.Domain.Common.Models;

public abstract class AggregateRoot<TId, TIdType> : Entity<TId>
  where TId : AggregateRootId<TIdType>
{
  public new AggregateRootId<TIdType> Id { get; protected set; }
  
  protected AggregateRoot(TId id)
  {
    Id = id;
  }



#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    protected AggregateRoot(){}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.



}