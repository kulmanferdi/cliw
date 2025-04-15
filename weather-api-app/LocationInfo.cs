using System.Text.Json;

namespace weather_api_app;

public class LocationInfo(JsonElement location)
{
    private string City { get; } = location.GetProperty("name").GetString()!;
    private string Country { get; } = location.GetProperty("country").GetString()!;
    private string LocalTime { get; } = location.GetProperty("localtime").GetString()!;

    public void Print()
    {
        Console.WriteLine($"Your location:\n" +
                          $"City: {City}\n" +
                          $"Country: {Country}\n" +
                          $"Local time: {LocalTime}\n");
    }

    public void GetCityAndCountry()
    {
        Console.WriteLine($"City: {City}, Country: {Country}");
    }
    public string CityName => City;
}
