using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using WeatherServiceDemo.Interfaces;

namespace WeatherServiceDemo.Services
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
