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
        Console.WriteLine($"Your location is {location}.\n");

        using var client = new HttpClient();
        var requestType = "current";
        var requestUrl = $"{BaseUrl}/{requestType}?access_key={apiKey}&query={Uri.EscapeDataString(location)}";
        try
        {
            var response = await client.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(content);
            var root = doc.RootElement;
            
            if (!root.TryGetProperty("current", out JsonElement bbbb))
            {
                Console.WriteLine("Error: 'current' data not found in response.");
                return;
            }
            if (root.TryGetProperty("current", out JsonElement current))
            {
                var temperature = current.GetProperty("temperature").GetInt32();
                var feelsLike = current.GetProperty("feelslike").GetInt32();
                var windSpeed = current.GetProperty("wind_speed").GetInt32();
                var windDirection = current.GetProperty("wind_dir").GetString();
                var humidity = current.GetProperty("humidity").GetInt32();
                var uvIndex = current.GetProperty("uv_index").GetInt32();
                var descriptions = current.GetProperty("weather_descriptions").EnumerateArray().Select(e => e.GetString()).ToArray();
                var observationTime = current.GetProperty("observation_time").GetString();
                
                Console.WriteLine("Weather info:");
                Console.WriteLine($"Temperature: {temperature} °C @ {observationTime}");
                Console.WriteLine($"Feels like: {feelsLike} °C\n{descriptions.FirstOrDefault()}");
                Console.WriteLine($"Wind: {windSpeed} km/h direction: {windDirection}");
                Console.WriteLine($"Humidity: {humidity} %\nUV Index: {uvIndex}\n");
            }
            else
            {
                Console.WriteLine("Error: Could not retrieve weather data.");
            }
            
            if (current.TryGetProperty("astro", out JsonElement astro))
            {
                var sunrise = astro.GetProperty("sunrise").GetString();
                var sunset = astro.GetProperty("sunset").GetString();
                var moonrise = astro.GetProperty("moonrise").GetString();
                var moonset = astro.GetProperty("moonset").GetString();
                var moonPhase = astro.GetProperty("moon_phase").GetString();
                
                Console.WriteLine("Astro info:");
                Console.WriteLine($"Sunrise: {sunrise}\nSunset: {sunset}");
                Console.WriteLine($"Moonrise: {moonrise}\nMoonset: {moonset}\nMoonPhase: {moonPhase}\n");
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