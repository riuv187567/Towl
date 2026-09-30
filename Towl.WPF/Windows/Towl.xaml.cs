using System.Windows;
using Towl.Core;
using Towl.Core.Data;
using Towl.Core.Data.Session;
using Towl.Core.Services;
using Towl.Core.Utils;
using Towl.WPF.Utils;

namespace Towl.WPF;

public partial class TowlWindow : Window
{
    private readonly VaultManager _state;
    private readonly DiscordIntegration _discord;
    private readonly CursorMovedBackgroundService _cursorMovedTest;

    public TowlWindow(VaultManager state, CursorMovedBackgroundService cursorMovedTest, DiscordIntegration discord)
    {
        InitializeComponent();

        _state = state;
        _discord = discord;
        _cursorMovedTest = cursorMovedTest;

        Loaded += async (a, b) => await RunTimerAsync();
    }

    private async Task RunTimerAsync()
    {
        var time = new PeriodicTimer(TimeSpan.FromSeconds(Constants.CycleSeconds));

        do
        {
            await UpdateMainTimer();
            await UpdateSecondaryTimer();
            await UpdateStatusBar();
        } while (await time.WaitForNextTickAsync());
    }

    private async Task UpdateMainTimer()
    {
        var shownProcName = _state.Current!.Settings.DisplayedProcessName;

        if (!_state.Current!.Data.ProcessEntryExist(shownProcName))
        {
            MainTimeText.Text = Constants.NoProcessDisplayedText;
            return;
        }

        var todayTimeString = TimeUtils.HumanizeTime(_state.Current!.Data.GetTodaySeconds(shownProcName));
        MainTimeText.Text = todayTimeString;
        _discord.SetDescription($"Tracked Time - {todayTimeString}"); // Todo: This should be moved to somewhere else
    }

    private async Task UpdateSecondaryTimer()
    {
        var shownProcName = _state.Current!.Settings.DisplayedProcessName;

        if (!_state.Current!.Data.ProcessEntryExist(shownProcName))
        {
            SecondaryTimeText.Text = Constants.NoProcessDisplayedText;
            return;
        }

        var todayTimeString = TimeUtils.HumanizeTime(_state.Current!.Data.GetTotalSeconds(shownProcName));
        SecondaryTimeText.Text = todayTimeString;
    }

    private async Task UpdateStatusBar()
    {
        var shownProcName = _state.Current!.Settings.DisplayedProcessName;

        if (!_state.Current!.Data.ProcessEntryExist(shownProcName))
            StatusBar.Fill = Constants.NotFoundColor.BrushFromDrawing();
        else
        {
            if (ProcessUtils.ProcessIsFocused(shownProcName) && _cursorMovedTest.CursorMoved)
                StatusBar.Fill = Constants.ActiveColor.BrushFromDrawing();
            else
                StatusBar.Fill = Constants.NotActiveColor.BrushFromDrawing();
        }
    }

    private void TowlWindowMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => DragMove();

    private void TowlWindowClose(object sender, RoutedEventArgs e) => Hide();

    private void TowlWindowMinimize(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
}
