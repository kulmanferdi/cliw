using System.Text.Json;
using System.Globalization;

namespace weather_api_app;

public class WeatherService(string apiKey)
{
    private readonly HttpClient _client = new();
    private const string BaseUrl = "https://api.weatherstack.com";

    private readonly string[] _queries = ["current", "forecast"];

    public async Task<(LocationInfo?, WeatherInfo?, AstroInfo?)> GetCurrentWeatherAsync(string location)
    {
        var url = $"{BaseUrl}/{_queries[0]}?access_key={apiKey}&query={Uri.EscapeDataString(location)}";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(content);
        var root = doc.RootElement;

        LocationInfo? locationInfo = null;
        WeatherInfo? weatherInfo = null;
        AstroInfo? astroInfo = null;

        if (root.TryGetProperty("location", out var locationJson))
            locationInfo = new LocationInfo(locationJson);

        if (root.TryGetProperty("current", out var currentJson))
        {
            weatherInfo = new WeatherInfo(currentJson);
            if (currentJson.TryGetProperty("astro", out var astroJson))
                astroInfo = new AstroInfo(astroJson);
        }

        return (locationInfo, weatherInfo, astroInfo);
    }

    public async Task<Forecast?> GetTomorrowForecastAsync(string location, int forecastDays, int forecastHours)
    {
        var url = $"{BaseUrl}/{_queries[1]}?access_key={apiKey}&query={Uri.EscapeDataString(location)}&forecast_days={forecastDays}&hourly={forecastHours}";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(content);
        var root = doc.RootElement;

        if (root.TryGetProperty("forecast", out var forecastJson))
        {
            var tomorrow = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (forecastJson.TryGetProperty(tomorrow, out var dayJson))
                return new Forecast(dayJson);
        }

        return null;
    }
}