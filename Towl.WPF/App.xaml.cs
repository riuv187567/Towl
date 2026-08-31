using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Towl.Core;
using Towl.Core.Data;
using Towl.Core.Services;

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

            static System.Drawing.Point mousePosition()
            {
                var point = Mouse.GetPosition(Application.Current.MainWindow);
                return new System.Drawing.Point((int)point.X, (int)point.Y);
            }

            builder.Services.AddHostedService(sp => new CursorMovedTestBackgroundService(mousePosition, state));

            builder.Services.AddSingleton<TowlWindow>();

            _host = builder.Build();
            _host.Start();

            var form = _host.Services.GetRequiredService<TowlWindow>();
            form.Show();

            _host.StopAsync().GetAwaiter().GetResult();
            _host.Dispose();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host!.Dispose();
            base.OnExit(e);
        }
    }
}
