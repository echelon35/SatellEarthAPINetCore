using Microsoft.AspNetCore.Mvc;
using SatellEarthAPI.Application.WeatherForecasts.Queries.GetWeatherForecasts;

namespace SatellEarthAPI.WebUI.Controllers
{
    public class WeatherForecastController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            return await Mediator.Send(new GetWeatherForecastsQuery());
        }
    }
}