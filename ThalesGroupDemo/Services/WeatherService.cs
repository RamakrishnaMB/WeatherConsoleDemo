using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WeatherServiceDemo.Interfaces;
using Formatting = Newtonsoft.Json.Formatting;

namespace WeatherServiceDemo.Services;

public class WeatherService : IWeatherService
{
    private readonly IWeatherApiConfiguration _weatherApiConfiguration;
    private readonly HttpClient _httpClient;

    public WeatherService(IWeatherApiConfiguration weatherApiConfiguration, HttpClient httpClient)
    {
        _weatherApiConfiguration = weatherApiConfiguration;
        _httpClient = httpClient;
    }


    public async Task FetchWeatherData()
    {
        var countries = _weatherApiConfiguration.GetCountries();
        var apiKey = _weatherApiConfiguration.GetApiKey();

        var tasks = new List<Task>();

        Parallel.ForEach(countries, country =>
        {
            var requestUri = GetRequestUri(country, apiKey);

            tasks.Add(_httpClient.GetStringAsync(requestUri)
                .ContinueWith(async task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        var weatherData = DeserializeWeatherData(task.Result);
                        var filePath = GetFilePath(country);
                        await WriteWeatherDataToFile(filePath, weatherData);
                    }
                }));
        });

        await Task.WhenAll(tasks);
    }

    private List<string> GetCountriesFromConfiguration(IConfiguration configuration)
    {
        return configuration.GetSection("WeatherApi:Countries").Get<List<string>>();
    }

    private string GetApiKeyFromConfiguration(IConfiguration configuration)
    {
        return configuration.GetValue<string>("WeatherApi:ApiKey");
    }

    private string GetRequestUri(string country, string? apiKey)
    {
        return $"https://api.weatherapi.com/v1/history.json?q={country}&dt={DateTime.Now.AddDays(-7):yyyy-MM-dd}&end_dt={DateTime.Now:yyyy-MM-dd}&lang=en&key={apiKey}";
    }

    private dynamic DeserializeWeatherData(string json)
    {
        return JsonConvert.DeserializeObject(json);
    }

    private string GetFilePath(string country)
    {
        return Path.Combine("ForcastOutput", $"{country}.json");
    }

    private Task WriteWeatherDataToFile(string filePath, dynamic weatherData)
    {
        return File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(weatherData, Formatting.Indented));
    }
}