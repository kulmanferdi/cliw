using System.Text.Json;

namespace weather_api_app;

public class WeatherInfo
{
    private string ObservationTime { get; set; }
    private int Temperature { get; set; }
    private int RealFeel { get; set; }
    private string [] WeatherDescriptions { get; set; }
    private int WindSpeed { get; set; }
    private string WindDirection { get; set; }
    private int Humidity { get; set; }
    private int Uvi { get; set; }

    public WeatherInfo(JsonElement current)
    {
        Temperature = current.GetProperty("temperature").GetInt32();
        RealFeel = current.GetProperty("feelslike").GetInt32();
        WindSpeed = current.GetProperty("wind_speed").GetInt32();
        WindDirection = current.GetProperty("wind_dir").GetString()!;
        Humidity = current.GetProperty("humidity").GetInt32();
        Uvi = current.GetProperty("uv_index").GetInt32();
        WeatherDescriptions = current.GetProperty("weather_descriptions").EnumerateArray().Select(e => e.GetString()).ToArray()!;
        ObservationTime = current.GetProperty("observation_time").GetString()!;
    }

    public void Print()
    {
        Console.WriteLine("Weather info:");
        Console.WriteLine($"Temperature: {Temperature} °C @ {ObservationTime}");
        Console.WriteLine($"Feels like: {RealFeel} °C\n{WeatherDescriptions.FirstOrDefault()}");
        Console.WriteLine($"Wind: {WindSpeed} km/h direction: {WindDirection}");
        Console.WriteLine($"Humidity: {Humidity} %\nUV Index: {Uvi}\n");

    }
    
}