namespace weather_api_app;

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DotNetEnv;

class Program
{
    private static readonly string BaseUrl = "https://api.weatherstack.com";

    static async Task Main(string [] args)
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
            
            if (!root.TryGetProperty("current", out JsonElement currentTry))
            {
                Console.WriteLine("Error: 'current' data not found in response.");
                return;
            }

            if (root.TryGetProperty("current", out JsonElement current))
            {
                WeatherInfo weatherInfo = new WeatherInfo(current);
                weatherInfo.Print();
            }
            else
            {
                Console.WriteLine("Error: Could not retrieve weather data.");
            }
            
            if (current.TryGetProperty("astro", out JsonElement astro))
            {
                AstroInfo astroInfo = new AstroInfo(astro);
                astroInfo.Print();
            }
            else
            {
                Console.WriteLine("Astro data not found.");
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching weather data: " + ex.Message);
        }
    }
}