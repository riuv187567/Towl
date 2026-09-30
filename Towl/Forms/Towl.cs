using Towl.Core;
using Towl.Core.Data;
using Towl.Core.Data.Session;
using Towl.Core.Services;
using Towl.Core.Utils;

namespace Towl;

public partial class Towl : Form
{
    private readonly TowlState _state;
    private readonly DiscordIntegration _discord;
    private readonly CursorMovedTestBackgroundService _cursorMovedTest;

    public Towl(TowlState state, CursorMovedTestBackgroundService cursorMovedTest, DiscordIntegration discord)
    {
        InitializeComponent();

        _state = state;
        _discord = discord;
        _cursorMovedTest = cursorMovedTest;
    }

    protected async override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await RunTimerAsync();
    }

    private async Task RunTimerAsync()
    {
        await UpdateTimer();
        var time = new PeriodicTimer(TimeSpan.FromSeconds(Constants.CycleSeconds));

        while (await time.WaitForNextTickAsync())
            await UpdateTimer();
    }

    private async Task UpdateTimer()
    {
        var displayedProcName = _state.Settings.DisplayedProcessName;
        if (!_state.Data.ProcessEntries.TryGetValue(_state.Settings.DisplayedProcessName, out var process))
        {
            sessionTime.BackColor = Constants.NotActiveColor;
            sessionTime.Text = Constants.NoProcessDisplayedText;
        }
        else
        {
            if (ProcessUtils.ProcessIsFocused(displayedProcName) && _cursorMovedTest.CursorMoved)
                sessionTime.BackColor = Constants.ActiveColor;
            else
                sessionTime.BackColor = Constants.NotActiveColor;

            var todayTimeString = TimeUtils.HumanizeTime(_state.Data.GetTodaySeconds(process.Name));
            sessionTime.Text = todayTimeString;
            _discord.SetDescription($"Tracked Time - {todayTimeString}");
        }
    }
}
