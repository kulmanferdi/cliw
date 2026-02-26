namespace weather_api_app;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .UseRazorConsole<weather_api_app.Components.Display>();

        var host = builder.Build();

        await host.RunAsync();
    }
}