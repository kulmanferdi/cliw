namespace weather_api_app;

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DotNetEnv;
using System.Globalization;

internal abstract class Program
{
    private const string BaseUrl = "https://api.weatherstack.com";

    private static readonly string [] Queries =
    [ 
        "current",
        "forecast"
    ];

    private const int ForecastDays = 1;
    private const int ForecastHours = 1;

    private static async Task Main(string?[] args)
    {
        Env.Load("./.env");
        var apiKey = Environment.GetEnvironmentVariable("API_KEY");
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            Console.WriteLine("API Key is missing");
            return;
        }
        
        string? location;
        if (args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
        {
            location = args[0];
        }
        else
        {
            Console.Write("Enter your location: ");
            location = Console.ReadLine();
        }
        if (string.IsNullOrWhiteSpace(location))
        {
            location = "Budapest, Hungary";
        }

        using var client = new HttpClient();
        
        var requestCurrentUrl = $"{BaseUrl}/{Queries[0]}?access_key={apiKey}&query={Uri.EscapeDataString(location)}";
        var requestForecastUrl = $"{BaseUrl}/{Queries[1]}?access_key={apiKey}&query={Uri.EscapeDataString(location)}&forecast_days={ForecastDays}&hourly={ForecastHours}";
        try
        {
            var response = await client.GetAsync(requestCurrentUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            await File.WriteAllTextAsync("current_weather.json", content);
            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;
            
            Console.Clear();
            Console.WriteLine("CLIW report\n");
            
            if (root.TryGetProperty("location", out var locationJsonElement))
            {
                var locationInfo = new LocationInfo(locationJsonElement);
                locationInfo.Print();
            }
            else
            {
                Console.WriteLine("Error: Could not retrieve location data.");
            }
            
            if (!root.TryGetProperty("current", out _))
            {
                Console.WriteLine("Error: 'current' data not found in response.");
                return;
            }
            
            if (root.TryGetProperty("current", out var currentJsonElement))
            {
                var weatherInfo = new WeatherInfo(currentJsonElement);
                weatherInfo.Print();
            }
            else
            {
                Console.WriteLine("Error: Could not retrieve weather data.");
            }
            
            if (currentJsonElement.TryGetProperty("astro", out var astroJsonElement))
            {
                var astroInfo = new AstroInfo(astroJsonElement);
                astroInfo.Print();
            }
            else
            {
                Console.WriteLine("Error: Could not retrieve astro data.");
            }
            
            response = await client.GetAsync(requestForecastUrl);
            response.EnsureSuccessStatusCode();
            content = await response.Content.ReadAsStringAsync();
            using var forecastDoc = JsonDocument.Parse(content);
            root = forecastDoc.RootElement;
            
            if (root.TryGetProperty("forecast", out var forecastJsonElement))
            {
                var tomorrow = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
                if (forecastJsonElement.TryGetProperty(tomorrow.ToString(CultureInfo.InvariantCulture),
                        out var dayJsonElement))
                {
                    var nextDay = new Forecast(dayJsonElement);
                    nextDay.Print();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching weather data: " + ex.Message);
        }
    }
}