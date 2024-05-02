﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherServiceDemo.Interfaces;
using WeatherServiceDemo.Services;


var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json",
        false,
        true);


IConfiguration configuration = builder.Build();
var services = new ServiceCollection();
services.AddSingleton(configuration);
services.AddSingleton<HttpClient>();
services.AddTransient<IWeatherApiConfiguration, WeatherApiConfiguration>();
services.AddTransient<IWeatherService, WeatherService>();

var serviceProvider = services.BuildServiceProvider();

var weatherService = serviceProvider.GetRequiredService<IWeatherService>();
var frequencyInMinutes = configuration.GetValue<int>("WeatherApi:FrequencyInMinutes");
Console.WriteLine("Weather data fetcher is running.");
// Set up the timer to fetch weather data
var timer = new Timer(async _ => await weatherService.FetchWeatherData());

ManualResetEvent quitEvent = new ManualResetEvent(false);

Console.CancelKeyPress += (sender, eArgs) => {
    quitEvent.Set();
    eArgs.Cancel = true;
};

timer?.Change(TimeSpan.Zero, TimeSpan.FromMinutes(frequencyInMinutes));

// Blocks the main thread until the quit event is triggered
quitEvent.WaitOne();

// Properly dispose of the timer when exiting
timer?.Dispose();