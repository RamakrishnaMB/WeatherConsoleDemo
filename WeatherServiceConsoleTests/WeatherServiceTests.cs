using Moq;
using Moq.Protected;
using System.Net;
using WeatherServiceDemo.Services;

namespace WeatherServiceConsoleTests
{
    public class WeatherServiceTests
    {
        [Fact]
        public async Task FetchWeatherData_CallsCorrectApiWithCorrectParameters()
        {
            // Arrange
            var weatherApiConfiguration = new TestWeatherApiConfiguration();

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                                  .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                                  .ReturnsAsync(new HttpResponseMessage
                                  {
                                      StatusCode = HttpStatusCode.OK,
                                      Content = new StringContent("{}")
                                  });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var weatherService = new WeatherService(weatherApiConfiguration, httpClient);

            // Act
            await weatherService.FetchWeatherData();

            // Assert
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync", Times.Exactly(2), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task FetchWeatherData_WritesWeatherDataToFile()
        {
            // Arrange
            var forecastOutputDirectory = "ForcastOutput";
            Directory.CreateDirectory(forecastOutputDirectory); // Create the directory

            var weatherApiConfiguration = new TestWeatherApiConfiguration();

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                                  .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                                  .ReturnsAsync(new HttpResponseMessage
                                  {
                                      StatusCode = HttpStatusCode.OK,
                                      Content = new StringContent("{}")
                                  });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var weatherService = new WeatherService(weatherApiConfiguration, httpClient);

            // Act
            await weatherService.FetchWeatherData();

            // Assert
            Console.WriteLine($"Checking directory: {forecastOutputDirectory}");
            var files = Directory.GetFiles(forecastOutputDirectory);
            foreach (var file in files)
            {
                Console.WriteLine($"Found file: {file}");
            }
            Assert.True(files.Length > 0, $"No files found in directory '{forecastOutputDirectory}'.");
        }
    }
}