using System.Text.Json;

namespace weather_api_app;

public class Forecast(JsonElement  root)
{
    private string MinTemp  { get; } = root.GetProperty("mintemp").GetString()!;
    private string MaxTemp  { get; } = root.GetProperty("maxtemp").GetString()!;
    private string AvgTemp  { get; }  = root.GetProperty("avgtemp").GetString()!;
    private int UVIndex  { get; } = root.GetProperty("uv_index").GetInt32();

    public void Print()
    {
        Console.WriteLine($"Forecast for tomorrow:\n" +
                          $"Min temp: {MinTemp} °C\n" +
                          $"Max temp: {MaxTemp} °C\n" +
                          $"Avg temp: {AvgTemp} °C\n" +
                          $"UV Index: {UVIndex}");
    }
    
}