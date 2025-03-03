# The Identity Paradox  DDD EF Core & Strongly Typed IDs

[Back][1]

1. Add new [AggregateRootId][2]
```csharp
namespace Apps.Domain.Common.Models;
public abstract class AggregateRootId<TId> : ValueObject
{
  public abstract TId Value { get; protected set; }
  
}
```
2. Modify [AggregateRoot][3]
From
```csharp
public abstract class AggregateRoot<TId> : Entity<TId>
  where TId : notnull
{
  protected AggregateRoot(TId id) : base(id){}
}
```
To
```csharp
public abstract class AggregateRoot<TId, TIdType> : Entity<TId>
  where TId : AggregateRootId<TIdType>
{
  public new AggregateRootId<TIdType> Id { get; protected set; }
  
  protected AggregateRoot(TId id)
  {
    Id = id;
  }
}
```
3. Replace ValueObject extension to use [AggregateRootId][2] classs instead
Sample
From
```csharp
public sealed class MenuId : ValueObject
{
  public Guid Value { get; private set; }
  ...
}
```
To
```csharp
public sealed class MenuId : AggregateRootId<Guid>
{
  public override Guid Value { get; protected set; }
  ...
}
```
4. Modify class using root [AggregateRootId][2] to specify the type of the Id
From
```csharp
public sealed class Menu : AggregateRoot<MenuId>
{
  ...
}
```
To
```csharp
public sealed class Menu : AggregateRoot<MenuId ,Guid>
{
  ...
}
```


[Top][0] | [Back to main][1]

[0]:#the-identity-paradox--ddd-ef-core--strongly-typed-ids
[1]:../../readme.md#contents
[2]:../../Apps/03-Apps.Domain/Common/Models/AggregateRootId.cs
[3]:../../Apps/03-Apps.Domain/Common/Models/AggregateRoot.cs