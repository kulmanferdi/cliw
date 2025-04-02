namespace weather_api_app;

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DotNetEnv;

class Program
{
    private static readonly string BaseUrl = "https://api.weatherstack.com";

    static async Task Main()
    {
        Env.Load("./.env");
        var apiKey = Environment.GetEnvironmentVariable("API_KEY");
        
        Console.Write("Enter your location: ");
        var location = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(location))
        {
            location = "Budapest";
        }

        using var client = new HttpClient();
        const string requestType = "current";
        var requestUrl = $"{BaseUrl}/{requestType}?access_key={apiKey}&query={Uri.EscapeDataString(location)}";
        try
        {
            var response = await client.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(content);
            var root = doc.RootElement;
            
            if (root.TryGetProperty("location", out JsonElement locationJsonElement))
            {
                LocationInfo locationInfo = new LocationInfo(locationJsonElement);
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

            if (root.TryGetProperty("current", out JsonElement currentJsonElement))
            {
                WeatherInfo weatherInfo = new WeatherInfo(currentJsonElement);
                weatherInfo.Print();
            }
            else
            {
                Console.WriteLine("Error: Could not retrieve weather data.");
            }
            
            if (currentJsonElement.TryGetProperty("astro", out JsonElement astroJsonElement))
            {
                AstroInfo astroInfo = new AstroInfo(astroJsonElement);
                astroInfo.Print();
            }
            else
            {
                Console.WriteLine("Error: Could not retrieve astro data.");
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching weather data: " + ex.Message);
        }
    }
}