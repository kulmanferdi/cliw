using System.Text.Json;

namespace weather_api_app;

public class ForecastHour(JsonElement hour)
{
    private string Time { get; } = hour.GetProperty("time").GetString()!;
    private int Temperature { get; } = hour.GetProperty("temperature").GetInt32();
    private int WindSpeed { get; } = hour.GetProperty("wind_speed").GetInt32();
    private string WindDirection { get; } = hour.GetProperty("wind_dir").GetString()!;
    private int Humidity { get; } = hour.GetProperty("humidity").GetInt32();
    private int ChanceOfRain { get; } = hour.GetProperty("chanceofrain").GetInt32();
    private int Uvi { get; } = hour.GetProperty("uv_index").GetInt32();

    public void Display()
    {
        Console.WriteLine($"Time: {Time}");
        Console.WriteLine($"Temperature: {Temperature} °C");
        Console.WriteLine($"Wind: {WindSpeed} km/h, direction: {WindDirection}");
        Console.WriteLine($"Humidity: {Humidity} %s");
        Console.WriteLine($"Chance of rain: {ChanceOfRain} %");
        Console.WriteLine($"UV index: {Uvi}");
        
    }
}