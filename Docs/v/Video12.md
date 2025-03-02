# Implementing AggregateRoot Entity ValueObject

[Back][1]

### Create Base Class

1. Create [Value Object Base Class][2]
```csharp
namespace Apps.Domain.Common.Models;

public abstract class ValueObject : IEquatable<ValueObject>
{
  public abstract IEnumerable<object> GetEqualityComponents();
  public override bool Equals(object? obj)
  {
      if(obj is null && obj?.GetType() != GetType() )
      {
        return false;
      }
      var valueObject = (ValueObject)obj;

      return valueObject.GetEqualityComponents()
        .SequenceEqual(valueObject.GetEqualityComponents());
  }
  public static bool operator ==(ValueObject left, ValueObject right)
  {
    return Equals(left, right);
  }
  public static bool operator !=(ValueObject left, ValueObject right)
  {
    return !Equals(left, right);
  }

  public override int GetHashCode()
  {
    return GetEqualityComponents()
      .Select(x => x?.GetHashCode() ?? 0)
      .Aggregate((x,y) => x ^ y);
  }

    public bool Equals(ValueObject? other)
    {
        return Equals((object?)other);
    }
} 
```

2. Create [Entity Base Class][3]
```csharp
namespace Apps.Domain.Common.Models;

public abstract class Entity<TId> : IEquatable<Entity<TId>>
  where TId : notnull
{
  public TId Id { get; protected set; }
  protected Entity(TId id){
    Id = id;
  }

  public override bool Equals(object? obj)
  {
      return obj is Entity<TId> entity && Id.Equals(Id);
  }

  public bool Equals(Entity<TId>? other)
  {
      return Equals((object?)other);
  }

  public static bool operator ==(Entity<TId> left, Entity<TId> right)
  {
    return Equals(left, right);
  }
  public static bool operator !=(Entity<TId> left, Entity<TId> right)
  {
    return !Equals(left, right);
  }
  public override int GetHashCode()
  {
      return Id.GetHashCode();
  }
}
```

3. Create [Aggregate Root Base Class][4]
```csharp
namespace Apps.Domain.Common.Models;

public abstract class AggregateRoot<TId> : Entity<TId>
  where TId : notnull
{
  protected AggregateRoot(TId id) : base(id){}
}
```

[Top][0] | [Back to main][1]

[0]:#implementing-aggregateroot-entity-valueobject
[1]:../../readme.md#contents
[2]:../../Apps/03-Apps.Domain/Common/Models/ValueObject.cs
[3]:../../Apps/03-Apps.Domain/Common/Models/Entity.cs
[4]:../../Apps/03-Apps.Domain/Common/Models/AggregateRoot.cs