using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using Towl.Core;
using Towl.Core.Data;
using Towl.Core.Services;
using Towl.Core.Utils;

namespace Towl.WPF
{
    public partial class App : Application
    {
        private IHost? _host;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

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

            builder.Services.AddHostedService(sp => new CursorMovedTestBackgroundService(ProcessUtils.GetCursorPosition, state));

            builder.Services.AddSingleton<TowlWindow>();

            _host = builder.Build();
            _host.Start();

            var form = _host.Services.GetRequiredService<TowlWindow>();
            form.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host!.StopAsync().GetAwaiter().GetResult();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}
