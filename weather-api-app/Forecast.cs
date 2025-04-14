using System.Text.Json;

namespace weather_api_app;

public class Forecast(JsonElement root)
{
    private string MinTemp  { get; } = root.GetProperty("mintemp").GetString()!;
    private string MaxTemp  { get; } = root.GetProperty("maxtemp").GetString()!;
    private string AvgTemp  { get; }  = root.GetProperty("avgtemp").GetString()!;
    private int Uvi  { get; } = root.GetProperty("uv_index").GetInt32();
    private List<ForecastHour> Hourly  { get; } = [];

    public void Print()
    {
        Console.WriteLine($"Forecast for tomorrow:\n" +
                          $"Min temp: {MinTemp} °C\n" +
                          $"Max temp: {MaxTemp} °C\n" +
                          $"Avg temp: {AvgTemp} °C\n" +
                          $"UV Index: {Uvi}");
        GetHourly();
        Console.WriteLine("Forecast hours:");
        Hourly.ForEach(h => h.Print());
    }

    private void GetHourly()
    {
        if (root.TryGetProperty("hourly", out JsonElement hourArray))
        {
            foreach (var hour in hourArray.EnumerateArray())
            {
                Hourly.Add(new ForecastHour(hour));
            }
        }
    }
}