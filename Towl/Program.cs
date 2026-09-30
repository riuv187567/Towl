using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Towl.Core;
using Towl.Core.Data;
using Towl.Core.Services;

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

        var towlStore = new TowlDataStore();

        var state = new TowlState()
        {
            Data = towlStore.LoadData(),
            Settings = towlStore.LoadSettings(),
        };

        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddSingleton(state);
        builder.Services.AddSingleton<IDataStore>(towlStore);

        builder.Services.AddSingleton<DiscordIntegration>();
        builder.Services.AddHostedService<TimerBackgroundService>();
        builder.Services.AddHostedService(sp => new CursorMovedTestBackgroundService(() => Cursor.Position));

        builder.Services.AddSingleton<Towl>();

        var host = builder.Build();
        host.Start();

        var form = host.Services.GetRequiredService<Towl>();
        Application.Run(form);

        host.StopAsync().GetAwaiter().GetResult();
        host.Dispose();
    }
}