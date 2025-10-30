namespace weather_api_app;

internal abstract class Program
{
    public static async Task Main()
    {
        //testing build pipeline
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        
        Log.Information("Starting application.");
        
        Env.Load("./.env");
        Log.Information("Setting up the environment...");
        
        var service = new WeatherService();
        
        await service.CheckEnvironmentVariables();
        
        Log.Information("Weather service is ready.");
        Console.Write("Enter your location: ");
        var location = Console.ReadLine();
        //await service.SetLocationAsync(location);
        try
        {
            if (location != null)
            {
                Log.Information("Location set");
                var (locationInfo, weatherInfo, astroInfo) = await service.GetCurrentWeatherAsync(location);
                //var forecast = await service.GetTomorrowForecastAsync(location);

            
                Log.Information("Creating report...");
            
                Console.Clear();
            
                Console.WriteLine("CLIW report\n");
            
                Console.WriteLine("Your location:");
                locationInfo?.Display();
                weatherInfo?.Display();
                astroInfo?.Display();
                //forecast?.Display();
                
                
            }
            Log.Information("Weather report successful.");
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Log.Information($"Current: version: {version}");
        }
        catch (Exception ex)
        {
            Log.Error("{0}Error fetching weather data: " + ex.Message, "ARG0");
        }
    }
}