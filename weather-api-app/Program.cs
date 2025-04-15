﻿namespace weather_api_app;

using System;
using System.Threading.Tasks;
using DotNetEnv;
using Serilog;

internal abstract class Program
{
    private static async Task Main()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        
        Log.Information("Starting application");
        Env.Load("./.env");
        var apiKey = Environment.GetEnvironmentVariable("API_KEY");
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            Log.Error("API Key is missing");
            return;
        }
        
        var service = new WeatherService(apiKey);
        
        Console.Write("Enter your location: ");
        var location = Console.ReadLine();
        //await service.SetLocationAsync(location);
        Log.Information("Location set");
        try
        {
            if (location != null)
            {
                var (locationInfo, weatherInfo, astroInfo) = await service.GetCurrentWeatherAsync(location);
                //var forecast = await service.GetTomorrowForecastAsync(location);

                Console.Clear();
                Console.WriteLine("CLIW report\n");

                locationInfo?.Print();
                weatherInfo?.Print();
                astroInfo?.Print();
                //forecast?.Print();
            }

            Log.Information("Weather report successful.");
        }
        catch (Exception ex)
        {
            Log.Error("Error fetching weather data: " + ex.Message);
        }
    }
}