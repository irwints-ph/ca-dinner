using Apps.Domain.Common.Models;
using Apps.Domain.Common.ValueObjects;
using Apps.Domain.DinnerAggregate.ValueObjects;
using Apps.Domain.HostAggregate.ValueObjects;
using Apps.Domain.MenuAggregate.Entities;
using Apps.Domain.MenuAggregate.ValueObjects;
using Apps.Domain.MenuReviewAggregate.ValueObjects;

namespace Apps.Domain.MenuAggregate;

public sealed class Menu : AggregateRoot<MenuId>
{
  private readonly List<MenuSection> _menuSection = new ();
  private readonly List<DinnerId> _dinnerIds = new ();
  private readonly List<MenuReviewId> _menuReviewIds = new ();

  public string Name { get; private set; }
  public string Description { get; private set; }
  public AverageRating AverageRating { get; private set; }
  public HostId HostId { get; private set; }
  public DateTime CreateDateTime { get; private set; }
  public DateTime UpdateDateTime { get; private set; }

  public IReadOnlyList<MenuSection> Sections => _menuSection.AsReadOnly();
  public IReadOnlyList<DinnerId> DinnerIds => _dinnerIds.AsReadOnly();
  public IReadOnlyList<MenuReviewId> MenuReviewIds => _menuReviewIds.AsReadOnly();

  public Menu(
    MenuId menuId,
    string name,
    string description,
    HostId hostId,
    List<MenuSection> menuSection,
    DateTime createDateTime,
    DateTime updateDateTime,
    AverageRating averageRating
    ) : base(menuId)
  {
    Name = name;
    Description = description;
    HostId = hostId;
    
    _menuSection.AddRange(menuSection);
    // menuSection.ForEach(m => {
    //   _menuSection.Add(m);
    // });
    

    CreateDateTime = createDateTime;
    UpdateDateTime = updateDateTime;
    AverageRating = averageRating;
    
  }

  public static Menu Create(
    HostId hostId,
    string name,
    string description,
    List<MenuSection> menuSection
    )
  {
    return new (
      MenuId.CreateUniqe(),
      name,
      description,
      hostId,
      menuSection,

      DateTime.UtcNow,
      DateTime.UtcNow,
      AverageRating.CreateNew()
    );

  }
  #pragma warning disable CS8618
  private Menu(){}
  #pragma warning restore CS8618

}