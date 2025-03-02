using Apps.Application.Menus.Commands.CreateMenu;
using Apps.Contracts.Menus;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Apps.Api.Controllers;
[Route("hosts/{hostid}/menus")]
public class MenuController : ApiController
{
  private readonly IMapper _mapper;
  private readonly ISender _mediator;
    public MenuController(IMapper mapper, ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

  public async Task<IActionResult> CreateMenu(
    CreateMenuRequest request,
    string hostId
  )
  {
    var command = _mapper.Map<CreateMenuCommand>((request,hostId));
    var createMenuResult = await _mediator.Send(command);

    // return createMenuResult.Match(
    //   a => CreatedAtAction(nameof(GetMenu), new {hostId, menuId = a.Id}, a),
    //   e => Problem(e)
    // );
    return createMenuResult.Match(
      a => Ok(_mapper.Map<MenuResponse>(a)),
      e => Problem(e)
    );

    // return Ok(request);
  }
}