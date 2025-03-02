using Apps.Domain.Common.Models;
using Apps.Domain.MenuAggregate.ValueObjects;

namespace Apps.Domain.MenuAggregate.Entities;

public sealed class MenuSection : Entity<MenuSectionId>
{
  private readonly List<MenuItem> _items = new List<MenuItem>();

  public string Name { get; private set; }
  public string Description { get; private set; }

  public IReadOnlyList<MenuItem> Items => _items.AsReadOnly();
  public MenuSection(
      MenuSectionId menuSectionId,
      string name,
      string description,
      List<MenuItem> menuItems) 
    : base(menuSectionId)
  {
    Name = name;
    Description = description;
    // menuItems.ForEach(m => {
    //   _items.Add(m);
    // });
    _items.AddRange(menuItems);
  }

  public static MenuSection Create(string name, string description, List<MenuItem> menuItems)
  {
    return new (
      MenuSectionId.CreateUniqe(),
      name,
      description,
      menuItems
    );
  }
  #pragma warning disable CS8618
  private MenuSection(){}
  #pragma warning restore CS8618

}