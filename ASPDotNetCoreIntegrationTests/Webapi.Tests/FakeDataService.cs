public class FakeDataService : IDataService
{
    Dictionary<string, WeatherForecast> weatherForecasts = new Dictionary<string, WeatherForecast>()
    {
        { "London", new WeatherForecast(DateOnly.FromDateTime(DateTime.Now), 20, "Sunny") },
        { "New York", new WeatherForecast(DateOnly.FromDateTime(DateTime.Now), 15, "Cloudy") },
        { "Tokyo", new WeatherForecast(DateOnly.FromDateTime(DateTime.Now), 30, "Rainy") },
    };

    public WeatherForecast GetWeatherByCity(string city)
    {
        // Simulate a data service that fetches weather data by city
        return weatherForecasts.ContainsKey(city) ? weatherForecasts[city] : null;
    }
}
