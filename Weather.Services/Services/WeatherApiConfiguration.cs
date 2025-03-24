using System.Diagnostics.CodeAnalysis;
using Weather.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Weather.Services.Services
{
    [ExcludeFromCodeCoverage]
    public class WeatherApiConfiguration(IConfiguration configuration) : IWeatherApiConfiguration
    {
        public IEnumerable<string>? GetCountries()
        {
            return configuration.GetSection("WeatherApi:Countries").Get<List<string>>();
        }

        public string? GetApiKey()
        {
            return configuration.GetValue<string>("WeatherApi:ApiKey");
        }
    }
}
