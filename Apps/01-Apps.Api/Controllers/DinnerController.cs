using Microsoft.AspNetCore.Mvc;

namespace Apps.Api.Controllers;
[Route("[Controller]")]
public class DinnerController : ApiController
{
  [HttpGet]
  
  public IActionResult ListDinners()
  {
    return Ok(Array.Empty<string>()); //return Ok([]);
  }
}