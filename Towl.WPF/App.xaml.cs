using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.ComponentModel;
using System.Windows;
using Towl.Core;
using Towl.Core.Data;
using Towl.Core.Services;
using Towl.Core.Utils;
using Application = System.Windows.Application;

namespace Towl.WPF
{
    public partial class App : Application
    {
        private NotifyIcon? _notifyIcon;
        private bool _isExit;
        private IHost? _host;
        private TowlWindow? _towlWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShutdownMode = ShutdownMode.OnExplicitShutdown;

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

            _towlWindow = _host.Services.GetRequiredService<TowlWindow>();
            _towlWindow.Closing += MainWindow_Closing;

            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.Icon = System.Drawing.SystemIcons.Application; // TODO: replace with the app's own icon
            _notifyIcon.Text = "Towl";
            _notifyIcon.DoubleClick += (s, args) => ShowMainWindow();
            _notifyIcon.Visible = true;

            CreateContextMenu();
        }

        private void CreateContextMenu()
        {
            _notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Open Towl").Click += (s, e) => ShowMainWindow();
            _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();
        }

        private void ExitApplication()
        {
            _isExit = true;
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            _towlWindow.Close();
            Application.Current.Shutdown();
        }

        private void ShowMainWindow()
        {
            if (_towlWindow.IsVisible)
            {
                if (_towlWindow.WindowState == WindowState.Minimized)
                    _towlWindow.WindowState = WindowState.Normal;

                _towlWindow.Activate();
            }
            else
            {
                _towlWindow.Show();
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!_isExit)
            {
                e.Cancel = true;
                _towlWindow.Hide(); // A hidden window can be shown again, a closed one not
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon?.Dispose();

            _host!.StopAsync().GetAwaiter().GetResult();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}
