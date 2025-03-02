using Apps.Application.Menus.Commands.CreateMenu;
using Apps.Contracts.Menus;
using Apps.Domain.MenuAggregate;
using Mapster;

using MenuSection = Apps.Domain.MenuAggregate.Entities.MenuSection;
using MenuItem = Apps.Domain.MenuAggregate.Entities.MenuItem;
// using Apps.Domain.DinnerAggregate.ValueObjects;

namespace Apps.Api.Common.Mapping;
public class MenuMappingConfig : IRegister
{
  public void Register(TypeAdapterConfig config)
  {
    config.NewConfig<(CreateMenuRequest request,string hostId),CreateMenuCommand>()
      .Map(d => d.HostId, s => s.hostId)
      .Map(d => d, s => s.request);
    config.NewConfig<Menu, MenuResponse>()
      .Map(d => d.Id, s => s.Id.Value)
      .Map(d => d.CreatedDateTime, s => s.CreateDateTime)
      .Map(d => d.UpdatedDateTime, s => s.UpdateDateTime)
      .Map(d => d.AverageRating, s => s.AverageRating.Value)
      .Map(d => d.HostId, s => s.HostId.Value)
      // .Map(d => d.Sections, s => s.Sections.Select(section => section.Id))
      .Map(d => d.DinnerIds, s => s.DinnerIds.Select(dinner => dinner.Value))
      .Map(d => d.MenuReviewIds, s => s.MenuReviewIds.Select(mr => mr.Value))

      // .Map(d => d.AverageRating, (
      //   s => s.AverageRating.NumRatings > 0 ? s.AverageRating.Value))
      ;
    config.NewConfig<MenuSection, MenuSectionResponse>()
      .Map(d => d.Id, s => s.Id.Value)
      // .Map(d => d.Name, s => s.Name)
      .Map(d => d.Item, s => s.Items)
      ;

    config.NewConfig<MenuItem, MenuItemResponse>()
      .Map(d => d.Id, s => s.Id.Value)
      // .Map(d => d.Name, s => s.Name)
      // .Map(d => d.Description, s => s.Description)
      ;

  }
}