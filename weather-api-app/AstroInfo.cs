namespace weather_api_app;

public class AstroInfo(JsonElement astro)
{
    private string Sunrise { get; } = astro.GetProperty("sunrise").GetString()!;
    private string Sunset { get; } = astro.GetProperty("sunset").GetString()!;
    private string Moonrise { get; } = astro.GetProperty("moonrise").GetString()!;
    private string Moonset { get; } = astro.GetProperty("moonset").GetString()!;
    private string Moonphase { get; } = astro.GetProperty("moon_phase").GetString()!;
    private int MoonIllumination { get; } = astro.GetProperty("moon_illumination").GetInt32();

    public void Display()
    {
        Console.WriteLine("Astro info:");
        Console.WriteLine($"Sunrise: {Sunrise}\nSunset: {Sunset}");
        Console.WriteLine($"Moonrise: {Moonrise}\nMoonset: {Moonset}\nMoonPhase: {Moonphase}, moon illumination: {MoonIllumination}\n");
    }
    
}