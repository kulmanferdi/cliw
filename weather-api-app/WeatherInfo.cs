using System.Text.Json;

namespace weather_api_app;

public class WeatherInfo(JsonElement current)
{
    private string ObservationTime { get; } = current.GetProperty("observation_time").GetString()!;
    private int Temperature { get; } = current.GetProperty("temperature").GetInt32();
    private int RealFeel { get; } =  current.GetProperty("feelslike").GetInt32();
    private string [] WeatherDescriptions { get; } = current.GetProperty("weather_descriptions").EnumerateArray().Select(e => e.GetString()).ToArray()!;
    private int WindSpeed { get; }= current.GetProperty("wind_speed").GetInt32();
    private string WindDirection { get; } = current.GetProperty("wind_dir").GetString()!;
    private int Humidity { get; } = current.GetProperty("humidity").GetInt32();
    private int Uvi { get; } =  current.GetProperty("uv_index").GetInt32();
    
    public void Print()
    {
        Console.WriteLine("Weather info:");
        Console.WriteLine($"Temperature: {Temperature} °C @ {ObservationTime}");
        Console.WriteLine($"Feels like: {RealFeel} °C\n{WeatherDescriptions.FirstOrDefault()}");
        Console.WriteLine($"Wind: {WindSpeed} km/h direction: {WindDirection}");
        Console.WriteLine($"Humidity: {Humidity} %\nUV Index: {Uvi}\n");

    }
    
}