using Microsoft.AspNetCore.Mvc;
using Weather.Services.Interfaces;

namespace Weather.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherService _weatherService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService)
    {
        _logger = logger;
        _weatherService = weatherService;
    }



    [HttpGet("FetchWeatherData")] // New HTTP GET endpoint
    public async Task<IActionResult> FetchWeatherData()
    {
        try
        {
            var weatherData = await _weatherService.FetchWeatherDataForApi(); // Call the service method
            return Ok(weatherData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching weather data.");
            return StatusCode(500, "An error occurred while fetching weather data.");
        }
    }
}
