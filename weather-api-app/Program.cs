namespace weather_api_app;

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    private static readonly string BaseUrl = "https://api.weatherstack.com";

    static async Task Main()
    {
        // register on weatherstack.com to get your free API key
        var apiKey = LoadApiKey();
        
        Console.Write("Enter your current location: ");
        var location = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(location))
        {
            Console.WriteLine("Invalid location.");
            return;
        }

        using var client = new HttpClient();
        var req = "current";
        var requestUrl = $"{BaseUrl}/{req}?access_key={apiKey}&query={Uri.EscapeDataString(location)}";

        try
        {
            var response = await client.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(content);
            using JsonDocument doc = JsonDocument.Parse(content);
            var root = doc.RootElement;
            
            if (root.TryGetProperty("current", out JsonElement current))
            {
                var temperature = current.GetProperty("temperature").GetInt32();
                var feelsLike = current.GetProperty("feelslike").GetInt32();
                var windSpeed = current.GetProperty("wind_speed").GetInt32();
                var humidity = current.GetProperty("humidity").GetInt32();
                var descriptions = current.GetProperty("weather_descriptions").EnumerateArray().Select(e => e.GetString()).ToArray();
                
                Console.WriteLine($"Temperature: {temperature} °C");
                Console.WriteLine($"Feels like: {feelsLike} °C\n{descriptions.FirstOrDefault()}");
                Console.WriteLine($"Wind: {windSpeed} km/h");
                Console.WriteLine($"Humidity: {humidity} %");
            }
            else
            {
                Console.WriteLine("Error: Could not retrieve weather data.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching weather data: " + ex.Message);
        }
    }
    
    static string LoadApiKey()
    {
        foreach (var line in File.ReadAllLines(".env"))
        {
            if (line.StartsWith("WEATHERSTACK_API_KEY="))
                return line.Split('=')[1].Trim();
        }
        return String.Empty;
    }
}