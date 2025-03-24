

namespace Weather.Domain.Models
{
    public class WeatherData
    {
        public Location? Location { get; set; }
        public Forecast? Forecast { get; set; }
    }

    public class Location
    {
        public string? Country { get; set; }
    }

    public class Forecast
    {
        public ForecastDay[]? Forecastday { get; set; }
    }

    public class ForecastDay
    {
        public string? Date { get; set; }
        public Hour[]? Hour { get; set; }
    }

    public class Hour
    {
        public string? Time { get; set; }
        public Condition? Condition { get; set; }
        public double Temp_c { get; set; }
        public double Temp_f { get; set; }
    }

    public class Condition
    {
        public string? Icon { get; set; }
        public string? Text { get; set; }
    }
}
