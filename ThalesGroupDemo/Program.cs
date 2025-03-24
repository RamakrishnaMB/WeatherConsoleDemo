using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weather.Services.Interfaces;
using Weather.Services.Services;


// Initialize the configuration builder
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json",
        false,
        true);

// Build the configuration
IConfiguration configuration = builder.Build();

// Initialize the service collection
var services = new ServiceCollection();

// Add configuration as a singleton
services.AddSingleton(configuration);

// Add HttpClient as a singleton
services.AddSingleton<HttpClient>();

// Add WeatherApiConfiguration and WeatherService as transient services
services.AddTransient<IWeatherApiConfiguration, WeatherApiConfiguration>();
services.AddTransient<IWeatherService, WeatherService>();

// Build the service provider
var serviceProvider = services.BuildServiceProvider();

// Get the WeatherService from the service provider
var weatherService = serviceProvider.GetRequiredService<IWeatherService>();

// Get the frequency in minutes from configuration
var frequencyInMinutes = configuration.GetValue<int>("WeatherApi:FrequencyInMinutes");

// Inform that the Weather data fetcher is running
Console.WriteLine("Weather data fetcher is running.");
Console.WriteLine();
// Set up the timer to fetch weather data
var timer = new Timer(async _ => await weatherService.FetchWeatherData());

// Create a ManualResetEvent for quitting
ManualResetEvent quitEvent = new ManualResetEvent(false);

// Set up event handler for Console Cancel Key Press
Console.CancelKeyPress += (sender, eArgs) =>
{
    quitEvent.Set();
    eArgs.Cancel = true;
};

// Start the timer with the specified frequency
timer?.Change(TimeSpan.Zero, TimeSpan.FromMinutes(frequencyInMinutes));

// Blocks the main thread until the quit event is triggered
quitEvent.WaitOne();

// Properly dispose of the timer when exiting
timer?.Dispose();