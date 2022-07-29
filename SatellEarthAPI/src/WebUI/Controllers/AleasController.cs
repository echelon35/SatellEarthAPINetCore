using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SatellEarthAPI.Application.Aleas.Queries.GetDisasters;

namespace SatellEarthAPI.WebUI.Controllers;

[Authorize]
public class AleasController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<DisastersVm>> Get()
    {
        return await Mediator.Send(new GetDisastersQuery());
    }
}
