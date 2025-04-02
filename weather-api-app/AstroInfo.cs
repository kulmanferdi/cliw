using System.Text.Json;

namespace weather_api_app;

public class AstroInfo
{
    private string Sunrise { get; set; }
    private string Sunset { get; set; }
    private string Moonrise { get; set; }
    private string Moonset { get; set; }
    private string Moonphase { get; set; }
    private int MoonIllumination { get; set; }

    public AstroInfo(JsonElement astro)
    {
        Sunrise = astro.GetProperty("sunrise").GetString()!;
        Sunset = astro.GetProperty("sunset").GetString()!;
        Moonrise = astro.GetProperty("moonrise").GetString()!;
        Moonset = astro.GetProperty("moonset").GetString()!;
        Moonphase = astro.GetProperty("moon_phase").GetString()!;
        MoonIllumination = astro.GetProperty("moon_illumination").GetInt32();
    }

    public void Print()
    {
        Console.WriteLine("Astro info:");
        Console.WriteLine($"Sunrise: {Sunrise}\nSunset: {Sunset}");
        Console.WriteLine($"Moonrise: {Moonrise}\nMoonset: {Moonset}\nMoonPhase: {Moonphase}, moon illumination: {MoonIllumination}\n");
    }
    
}