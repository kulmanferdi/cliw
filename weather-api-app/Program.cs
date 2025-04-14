namespace weather_api_app;

using System;
using System.Threading.Tasks;
using DotNetEnv;

internal abstract class Program
{
    private const int ForecastDays = 1;
    private const int ForecastHours = 1;

    private static async Task Main()
    {
        Env.Load("./.env");
        var apiKey = Environment.GetEnvironmentVariable("API_KEY");
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            Console.WriteLine("API Key is missing");
            return;
        }

        Console.Write("Enter your location: ");
        var location = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(location))
            location = "Budapest, Hungary";

        var service = new WeatherService(apiKey);

        try
        {
            var (locationInfo, weatherInfo, astroInfo) = await service.GetCurrentWeatherAsync(location);
            var forecast = await service.GetTomorrowForecastAsync(location, ForecastDays, ForecastHours);

            Console.Clear();
            Console.WriteLine("CLIW report\n");

            locationInfo?.Print();
            weatherInfo?.Print();
            astroInfo?.Print();
            forecast?.Print();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching weather data: " + ex.Message);
        }
    }
}