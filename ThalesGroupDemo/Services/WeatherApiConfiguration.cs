using Microsoft.Extensions.Configuration;
using WeatherServiceDemo.Interfaces;

namespace WeatherServiceDemo.Services
{
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
