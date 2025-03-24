using Weather.Domain.Models;

namespace Weather.Services.Interfaces;

public interface IWeatherService
{
    Task FetchWeatherData();

    Task<List<WeatherData>> FetchWeatherDataForApi();
}