using Apps.Application.Common.Interfaces.Persistence;
using Apps.Domain.HostAggregate.ValueObjects;
using Apps.Domain.MenuAggregate;
using Apps.Domain.MenuAggregate.Entities;
using ErrorOr;
using MediatR;

namespace Apps.Application.Menus.Commands.CreateMenu;
public class CreateMenuCommandHandler : IRequestHandler<CreateMenuCommand, ErrorOr<Menu>>
{
  private readonly IMenuRepository _menuRepository;

    public CreateMenuCommandHandler(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<ErrorOr<Menu>> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
  {
    //0 Temporary to remove warning
    await Task.CompletedTask;
    //Create Menu
    var menu = Menu.Create(
      HostId.Create(request.HostId),
      request.Name,
      request.Description,
      request.Sections.ConvertAll(s => MenuSection.Create(
        s.Name,
        s.Description,
        s.Items.ConvertAll(i => MenuItem.Create(
          i.Name,
          i.Description
        ))
      ))
    );
    //Presist Data
    _menuRepository.Add(menu);
    //Return Menu

    return menu;
  }
}