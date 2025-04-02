using System.Text.Json;

namespace weather_api_app;

public class LocationInfo(JsonElement location)
{
    private string City { get; } = location.GetProperty("name").GetString()!;
    private string Country { get; } = location.GetProperty("country").GetString()!;
    private string LocalTime { get; } = location.GetProperty("localtime").GetString()!;

    public void Print()
    {
        Console.WriteLine($"Your location:\nCity: {City}\nCountry: {Country}\nLocal time: {LocalTime}\n");
    }
}
