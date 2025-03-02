using Apps.Domain.Common.Models;
using Apps.Domain.MenuAggregate.ValueObjects;

namespace Apps.Domain.MenuAggregate.Entities;

public sealed class MenuItem : Entity<MenuItemId>
{
  public string Name { get; private set; }
  public string Description { get; private set; }
  public MenuItem(
    MenuItemId menuItemId,
    string name,
    string description
    )
    : base(menuItemId)
  {
    Name = name;
    Description = description;
  }
  public static MenuItem Create(string name, string description)
  {
    return new (
      MenuItemId.CreateUniqe(),
      name,
      description
    );
  }
  #pragma warning disable CS8618
  private MenuItem(){}
  #pragma warning restore CS8618

}