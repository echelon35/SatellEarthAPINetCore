using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SatellEarthAPI.Application.Common.Models;
using SatellEarthAPI.Application.Disasters.Queries.GetDisastersWithPagination;

namespace SatellEarthAPI.WebUI.Controllers;

[Authorize]
public class DisastersController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<DisasterBriefDto>>> GetDisastersWithPagination([FromQuery] GetDisastersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }
}
