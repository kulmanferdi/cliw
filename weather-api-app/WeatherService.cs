namespace weather_api_app;

public class WeatherService(int forecastDays, int forecastHours)
{
    private readonly HttpClient _client = new();
    
    private readonly string? _baseUrl = Environment.GetEnvironmentVariable("BASE_URL");
    private readonly string? _apiKey= Environment.GetEnvironmentVariable("API_KEY");

    private readonly string[] _queries = ["current", "forecast", "autocomplete"];

    public Task CheckEnvironmentVariables()
    {
        if (string.IsNullOrEmpty(_apiKey))
            Log.Error("API key is missing");
        if(string.IsNullOrEmpty(_baseUrl))
            Log.Error("Base URL is missing");
        return Task.CompletedTask;
    }

    public async Task<(LocationInfo?, WeatherInfo?, AstroInfo?)> GetCurrentWeatherAsync(string location)
    {
        var url = $"{_baseUrl}/{_queries[0]}?access_key={_apiKey}&query={Uri.EscapeDataString(location)}";
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

        if (!root.TryGetProperty("current", out var currentJson)) return (locationInfo, weatherInfo, astroInfo);
        weatherInfo = new WeatherInfo(currentJson);
        if (currentJson.TryGetProperty("astro", out var astroJson))
            astroInfo = new AstroInfo(astroJson);

        return (locationInfo, weatherInfo, astroInfo);
    }

    public async Task<Forecast?> GetTomorrowForecastAsync(string location)
    {
        var url = $"{_baseUrl}/{_queries[1]}?access_key={_apiKey}&query={Uri.EscapeDataString(location)}&forecast_days={forecastDays}&hourly={forecastHours}";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(content);
        var root = doc.RootElement;

        if (!root.TryGetProperty("forecast", out var forecastJson)) return null;
        var tomorrow = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return forecastJson.TryGetProperty(tomorrow, out var dayJson) ? new Forecast(dayJson) : null;
    }
    
    public async Task<string?> SetLocationAsync(string? location)
    {
        var locations = new List<LocationInfo?>();
        if (location != null)
        {
            var url = $"{_baseUrl}/{_queries[2]}?access_key={_apiKey}&query={Uri.EscapeDataString(location)}";
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
        
            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;
            if (!root.TryGetProperty("results", out var locationsJson))
            {
                locations.AddRange(locationsJson.EnumerateArray().Select(item => new LocationInfo(item)));
            }
        }
        else 
            location = "Budapest, Hungary";

        switch (locations.Count)
        {
            case 1: break;
            case > 1:
            {
                PrintLocationsList(locations);
                Console.Write("Choose your locations number: ");
                var number = Convert.ToInt32(Console.ReadLine());
                location = locations[number - 1]?.CityName;
            }
                break;
        }

        return location;
    }
    
    private static void PrintLocationsList(List<LocationInfo?> locations)
    {
        var i = 0;
        Console.WriteLine("List of locations:");
        foreach (var item in locations)
        {
            Console.Write($"{++i} - ");
            item?.Display();
        }
    }
}