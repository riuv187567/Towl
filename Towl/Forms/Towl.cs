using Towl.Data;
using Towl.Utils;

namespace Towl;

public partial class Towl : Form
{
    private readonly TowlState _state;
    private readonly DiscordIntegration _discord;

    public Towl(TowlState state, DiscordIntegration discord)
    {
        InitializeComponent();

        _state = state;
        _discord = discord;
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
        {
            await UpdateTimer();
            await Task.Delay(Constants.CycleSeconds);
            TowlDataManager.SaveData(_state.Data);
        }
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
            if (ProcessUtils.ProcessIsFocused(displayedProcName) && _state.CursorMoved)
                sessionTime.BackColor = Constants.ActiveColor;
            else
                sessionTime.BackColor = Constants.NotActiveColor;

            var todayTimeString = TimeUtils.HumanizeTime(_state.Data.GetTodaySeconds(process.Name));
            sessionTime.Text = todayTimeString;
            _discord.SetDescription($"Tracked Time - {todayTimeString}");
        }
    }
}
