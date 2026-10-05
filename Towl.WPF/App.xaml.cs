using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.ComponentModel;
using System.Windows;
using Towl.Core.Data;
using Towl.Core.Services;
using Towl.Core.Utils;
using Towl.WPF.Service;
using Application = System.Windows.Application;

namespace Towl.WPF;

public partial class App : Application
{
    private IHost? _host;
    private bool _isExit;

    private TowlWindow? _towlMainWindow;
    private NotifyIcon? _towlNotifyIcon;
    private IMessageDialogService? _errorDialog;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ShutdownMode = ShutdownMode.OnExplicitShutdown;

        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddSingleton<VaultManager>();
        builder.Services.AddSingleton<DiscordIntegration>();
        builder.Services.AddSingleton<IMessageDialogService, MessageDialogService>();

        builder.Services.AddSingleton(sp => new CursorMovedBackgroundService(ProcessUtils.GetCursorPosition));
        builder.Services.AddHostedService(sp => sp.GetRequiredService<CursorMovedBackgroundService>());

        builder.Services.AddHostedService<TimerBackgroundService>();
        builder.Services.AddHostedService<VaultBackupService>();
        builder.Services.AddSingleton<TowlWindow>();

        _host = builder.Build();
        _host.Start();

        _towlMainWindow = _host.Services.GetRequiredService<TowlWindow>();
        _towlMainWindow.Closing += (object? sender, CancelEventArgs e) =>
        {
            if (_isExit)
                return;

            e.Cancel = true;
            _towlMainWindow!.Hide();
        };

        _errorDialog = _host.Services.GetRequiredService<IMessageDialogService>();

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

        _towlNotifyIcon!.ContextMenuStrip = new ContextMenuStrip();
        _towlNotifyIcon.ContextMenuStrip.Items.Add("Open Towl").Click += (s, e) => ShowMainWindow();
        _towlNotifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();
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

    private void ExitApplication()
    {
        _isExit = true;

        _towlNotifyIcon!.Visible = false;
        _towlNotifyIcon.Dispose();

        _towlMainWindow!.Close();

        Current.Shutdown();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _towlNotifyIcon?.Dispose();

        var vaultManager = _host!.Services.GetRequiredService<VaultManager>();
        _host!.StopAsync().GetAwaiter().GetResult();

        try
        {
            if (vaultManager.Current.SavingEnabled)
                vaultManager._storage.SaveVault(vaultManager.Current.Data, vaultManager.Current.Settings);
        }
        catch
        {
            _errorDialog!.ShowErrorMessage("Failed to perform vault backup");
        }

        _host.Dispose();

        base.OnExit(e);
    }
}
