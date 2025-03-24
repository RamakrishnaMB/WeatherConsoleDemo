namespace Weather.Services.Interfaces;

public interface IWeatherApiConfiguration
{
    IEnumerable<string>? GetCountries();
    string? GetApiKey();
}