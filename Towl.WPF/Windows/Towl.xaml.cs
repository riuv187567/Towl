using System.Windows;
using Towl.Core;
using Towl.Core.Data;
using Towl.Core.Data.Session;
using Towl.Core.Utils;
using Towl.WPF.Utils;

namespace Towl.WPF;

public partial class TowlWindow : Window
{
    private readonly TowlState _state;
    private readonly DiscordIntegration _discord;

    public TowlWindow(TowlState state, DiscordIntegration discord)
    {
        InitializeComponent();

        _state = state;
        _discord = discord;

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
        var displayedProcName = _state.Settings.DisplayedProcessName;
        if (!_state.Data.ProcessEntries.TryGetValue(displayedProcName, out var process))
        {
            MainTimeText.Text = Constants.NoProcessDisplayedText;
            return;
        }

        var todayTimeString = TimeUtils.HumanizeTime(_state.Data.GetTodaySeconds(process.Name));
        MainTimeText.Text = todayTimeString;
        _discord.SetDescription($"Tracked Time - {todayTimeString}"); // Todo: This should be moved to somewhere else
    }

    private async Task UpdateSecondaryTimer()
    {
        if (!_state.Data.ProcessEntries.TryGetValue(_state.Settings.DisplayedProcessName, out var process))
        {
            SecondaryTimeText.Text = Constants.NoProcessDisplayedText;
            return;
        }

        var todayTimeString = TimeUtils.HumanizeTime(process.TotalSeconds);
        SecondaryTimeText.Text = todayTimeString;
    }

    private async Task UpdateStatusBar()
    {
        var displayedProcName = _state.Settings.DisplayedProcessName;

        if (!_state.Data.ProcessEntries.TryGetValue(displayedProcName, out var process))
            StatusBar.Fill = Constants.NotFoundColor.BrushFromDrawing();
        else
        {
            if (ProcessUtils.ProcessIsFocused(displayedProcName) && _state.CursorMoved)
                StatusBar.Fill = Constants.ActiveColor.BrushFromDrawing();
            else
                StatusBar.Fill = Constants.NotActiveColor.BrushFromDrawing();
        }
    }

    private void TowlWindowMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => DragMove();

    private void TowlWindowClose(object sender, RoutedEventArgs e) => Hide();

    private void TowlWindowMinimize(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
}
