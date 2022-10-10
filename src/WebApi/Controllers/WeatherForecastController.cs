using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.WeatherForecasts.Queries.GetWeatherForecasts;

namespace WebApi.Controllers;

public class WeatherForecastController : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get() => 
        await Mediator.Send(new GetWeatherForecastsQuery());
}