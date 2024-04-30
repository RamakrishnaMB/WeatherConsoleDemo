namespace WeatherServiceDemo.Interfaces;

public interface IWeatherApiConfiguration
{
    IEnumerable<string>? GetCountries();
    string? GetApiKey();
}