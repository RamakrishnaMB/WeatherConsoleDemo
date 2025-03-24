using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Weather.Domain.Models;
using Weather.Services.Interfaces;
using Formatting = Newtonsoft.Json.Formatting;

namespace Weather.Services.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherApiConfiguration _weatherApiConfiguration;
        private readonly HttpClient _httpClient;
        private string projectDirectory = Directory.GetCurrentDirectory();

        // Constructor to initialize WeatherService with required dependencies
        public WeatherService(IWeatherApiConfiguration weatherApiConfiguration, HttpClient httpClient)
        {
            _weatherApiConfiguration = weatherApiConfiguration;
            _httpClient = httpClient;
        }

        // Method to fetch weather data asynchronously
        public async Task FetchWeatherData()
        {
            try
            {
                var countries = _weatherApiConfiguration.GetCountries();
                var apiKey = _weatherApiConfiguration.GetApiKey();
                Console.WriteLine("Retrieving Countries and API Key for Weather API " + DateTime.Now);
                Console.WriteLine();
                var tasks = new List<Task>();
                GenerateWeatherData(countries, apiKey, tasks);
                await Task.WhenAll(tasks);
                Console.WriteLine("Press Ctrl+C to exit or Close window to stop the program.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong. Please try again");
                Console.WriteLine(ex.Message);
            }
        }

        // Method to generate weather data for each country
        private void GenerateWeatherData(IEnumerable<string>? countries, string? apiKey, List<Task> tasks)
        {
            Console.WriteLine("Calling Weather History API at: " + DateTime.Now);
            _ = Parallel.ForEach(countries, country =>
            {
                var requestUri = GetRequestUriHistory(country, apiKey);

                tasks.Add(_httpClient.GetStringAsync(requestUri)
                    .ContinueWith(async task =>
                    {
                        if (task.IsCompletedSuccessfully)
                        {
                            var weatherData = DeserializeWeatherData(task.Result);
                            var filePath = GetFilePath(country, "weatherhistorydata");
                            await WriteWeatherDataToFile("weatherhistorydata", filePath, weatherData);
                        }
                    }));
            });

            Console.WriteLine("Json files for Weather History Created successfully at " + DateTime.Now);
            Console.WriteLine("For Generated file please check this path for \\bin\\Debug\\net8.0\\weatherhistorydata folder for Country wise json files");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Calling Weather Forecast API at: " + DateTime.Now);
            _ = Parallel.ForEach(countries, country =>
            {
                var requestUri = GetRequestUriForecast7Days(country, apiKey);

                tasks.Add(_httpClient.GetStringAsync(requestUri)
                    .ContinueWith(async task =>
                    {
                        if (task.IsCompletedSuccessfully)
                        {
                            var weatherData = DeserializeWeatherData(task.Result);
                            var filePath = GetFilePath(country, "weatherforecastdata");
                            await WriteWeatherDataToFile("weatherforecastdata", filePath, weatherData);
                        }
                    }));
            });

            Console.WriteLine("Json files for Weather Forecast Created successfully at " + DateTime.Now);
            Console.WriteLine("For Generated file please check this path for \\bin\\Debug\\net8.0\\weatherforecastdata folder for Country wise json files");
            Console.WriteLine();
            Console.WriteLine();
        }

        // Method to retrieve list of countries from configuration
        private List<string> GetCountriesFromConfiguration(IConfiguration configuration)
        {
            return configuration.GetSection("WeatherApi:Countries").Get<List<string>>();
        }

        // Method to retrieve API key from configuration
        private string GetApiKeyFromConfiguration(IConfiguration configuration)
        {
            return configuration.GetValue<string>("WeatherApi:ApiKey");
        }

        // Method to construct the request URI for historical weather data
        private string GetRequestUriHistory(string country, string? apiKey)
        {
            return $"https://api.weatherapi.com/v1/history.json?q={country}&dt={DateTime.Now.AddDays(-7):yyyy-MM-dd}&end_dt={DateTime.Now:yyyy-MM-dd}&lang=en&key={apiKey}";
        }

        // Method to construct the request URI for 7 days weather forecast
        private string GetRequestUriForecast7Days(string country, string? apiKey)
        {
            return $"https://api.weatherapi.com/v1/forecast.json?q={country}&days=7&key={apiKey}";
        }

        // Method to deserialize weather data from JSON
        private dynamic DeserializeWeatherData(string json)
        {
            return JsonConvert.DeserializeObject(json);
        }

        // Method to get file path for saving weather data
        private string GetFilePath(string country, string folderName)
        {
            return Path.Combine(projectDirectory, folderName, $"{country}.json");
        }

        // Method to write weather data to file asynchronously
        private Task WriteWeatherDataToFile(string folderName, string filePath, dynamic weatherData)
        {
            if (!Directory.Exists(Path.Combine(projectDirectory, folderName)))
            {
                Directory.CreateDirectory(Path.Combine(projectDirectory, folderName));
            }
            return File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(weatherData, Formatting.Indented));
        }

        public async Task<List<WeatherData>> FetchWeatherDataForApi()
        {
            var countries = _weatherApiConfiguration.GetCountries();
            var apiKey = _weatherApiConfiguration.GetApiKey();
            var tasks = new List<Task<string>>();
            var finalWeatherData = new List<WeatherData>();
            foreach (var country in countries)
            {
                var historyRequestUri = GetRequestUriHistory(country, apiKey);
                tasks.Add(_httpClient.GetStringAsync(historyRequestUri));

                var forecastRequestUri = GetRequestUriForecast7Days(country, apiKey);
                tasks.Add(_httpClient.GetStringAsync(forecastRequestUri));
            }
            await Task.WhenAll(tasks);

            List<dynamic> results = new List<dynamic>();
            foreach (var task in tasks)
            {
                if (task.IsCompletedSuccessfully)
                {
                    results.Add(DeserializeWeatherData(task.Result));
                }
            }
            foreach (var result in results)
            {
                var finalWeather = JsonConvert.DeserializeObject<WeatherData>(JsonConvert.SerializeObject(result));
                finalWeatherData.Add(finalWeather);
            }

            return finalWeatherData;
        }
    }
}
