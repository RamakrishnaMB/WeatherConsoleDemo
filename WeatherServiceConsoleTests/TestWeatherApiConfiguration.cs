using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherServiceDemo.Interfaces;

namespace WeatherServiceConsoleTests
{
    public class TestWeatherApiConfiguration : IWeatherApiConfiguration
    {
        public IEnumerable<string>? GetCountries()
        {
            return new List<string> { "US", "UK" };
        }

        public string? GetApiKey()
        {
            return "your_api_key";
        }
    }

}
