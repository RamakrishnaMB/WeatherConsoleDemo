using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherServiceDemo.Interfaces;
using WeatherServiceDemo.Services;


var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json",
        false,
        true);


var sourcePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
var destinationDirectory = Path.Combine(Directory.GetCurrentDirectory(), "bin", "Debug", "net8.0");

// Ensure the destination directory exists
if (!Directory.Exists(destinationDirectory))
    File.Copy(sourcePath, Path.Combine(destinationDirectory, "appsettings.json"), true);


IConfiguration configuration = builder.Build();

var services = new ServiceCollection();
services.AddSingleton(configuration);
services.AddSingleton<HttpClient>();
services.AddTransient<IWeatherApiConfiguration, WeatherApiConfiguration>();
services.AddTransient<IWeatherService, WeatherService>();

var serviceProvider = services.BuildServiceProvider();

var weatherService = serviceProvider.GetRequiredService<IWeatherService>();
var frequencyInMinutes = configuration.GetValue<int>("WeatherApi:FrequencyInMinutes");
Console.WriteLine("Weather data fetcher is running....");
// Set up the timer to fetch weather data
var timer = new Timer(async _ => await weatherService.FetchWeatherData(), null, TimeSpan.Zero,
    TimeSpan.FromMinutes(frequencyInMinutes));

Console.WriteLine(
    "Press Any Key To Exit and Check this path for bin\\Debug\\net8.0\\ForcastOutput folder for weather data");
Console.ReadKey();

// Clean up
timer?.Dispose();

