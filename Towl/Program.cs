using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Towl.Data;
using Towl.Services;

namespace Towl;

public static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var state = new TowlState()
        {
            Data = TowlDataManager.LoadData(),
            Settings = TowlDataManager.LoadSettings(),
            CursorMoved = false,
        };

        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddSingleton(state);

        builder.Services.AddSingleton<DiscordIntegration>();
        builder.Services.AddHostedService<TimerBackgroundService>();
        builder.Services.AddHostedService<CursorMovedTestBackgroundService>();

        builder.Services.AddSingleton<Towl>();

        var host = builder.Build();
        host.Start();

        var form = host.Services.GetRequiredService<Towl>();
        Application.Run(form);

        host.StopAsync().GetAwaiter().GetResult();
        host.Dispose();
    }
}