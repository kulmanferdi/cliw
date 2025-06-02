namespace weather_api_app;

public class LocationInfo(JsonElement location)
{
    private string City { get; } = location.GetProperty("name").GetString()!;
    private string Country { get; } = location.GetProperty("country").GetString()!;
    private string LocalTime { get; } = location.GetProperty("localtime").GetString()!;

    public void Display()
    {
        Console.WriteLine($"{City}, {Country} @ {LocalTime}\n");
    }
    
    public string CityName => City;
}
