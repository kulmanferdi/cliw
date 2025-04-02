using System.Text.Json;

namespace weather_api_app;

public class Forecast(JsonElement  root)
{
    private string MinTemp  { get; } = root.GetProperty("mintemp").GetString()!;
    private string MaxTemp  { get; } = root.GetProperty("maxtemp").GetString()!;
    private string AvgTemp  { get; }  = root.GetProperty("avgtemp").GetString()!;

    public void Print()
    {
        Console.WriteLine($"Forecast for tomorrow:\nMin temp: {MinTemp} °C\nMax temp: {MaxTemp} °C\nAvg temp: {AvgTemp} °C\n");
    }
    
}