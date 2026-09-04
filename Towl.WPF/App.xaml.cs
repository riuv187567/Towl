using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.ComponentModel;
using System.Windows;
using Towl.Core;
using Towl.Core.Data;
using Towl.Core.Services;
using Towl.Core.Utils;
using Application = System.Windows.Application;

namespace Towl.WPF;

public partial class App : Application
{
    private IHost? _host;
    private TowlWindow? _towlMainWindow;

    private NotifyIcon? _towlNotifyIcon;

    private bool _isExit;

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

        _towlMainWindow = _host.Services.GetRequiredService<TowlWindow>();
        _towlMainWindow.Closing += TowlWindowClosing;

        CreateContextMenu();
        ShowMainWindow();
    }

    private void CreateContextMenu()
    {
        _towlNotifyIcon = new NotifyIcon
        {
            Icon = SystemIcons.Application, // TODO: replace with the app's own icon
            Text = "Towl"
        };

        _towlNotifyIcon.DoubleClick += (s, args) => ShowMainWindow();
        _towlNotifyIcon.Visible = true;

        _towlNotifyIcon!.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
        _towlNotifyIcon.ContextMenuStrip.Items.Add("Open Towl").Click += (s, e) => ShowMainWindow();
        _towlNotifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();
    }

    private void ExitApplication()
    {
        _isExit = true;

        _towlNotifyIcon!.Visible = false;
        _towlNotifyIcon.Dispose();

        _towlMainWindow!.Close();

        Current.Shutdown();
    }

    private void ShowMainWindow()
    {
        if (!_towlMainWindow!.IsVisible)
        {
            _towlMainWindow.Show();
            return;
        }

        if (_towlMainWindow.WindowState == WindowState.Minimized)
            _towlMainWindow.WindowState = WindowState.Normal;

        _towlMainWindow.Activate();
    }

    private void TowlWindowClosing(object? sender, CancelEventArgs e)
    {
        if (_isExit)
            return;

        e.Cancel = true;
        _towlMainWindow!.Hide();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _towlNotifyIcon?.Dispose();

        _host!.StopAsync().GetAwaiter().GetResult();
        _host.Dispose();

        base.OnExit(e);
    }
}
