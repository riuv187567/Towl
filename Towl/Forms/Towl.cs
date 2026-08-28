using Towl.Data;
using Towl.Utils;

namespace Towl;

public partial class Towl : Form
{
    private readonly ApplicationState _state;
    private readonly DiscordIntegration _discord;

    public Towl(ApplicationState state, DiscordIntegration discord)
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
        var time = new PeriodicTimer(TimeSpan.FromSeconds(Constants.CycleSeconds));

        while (await time.WaitForNextTickAsync())
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

                sessionTime.Text = TimeUtils.HumanizeTime(process.TotalSeconds);
                _discord.SetDescription($"Tracked Time - {TimeUtils.HumanizeTime(process.TotalSeconds)}");
            }

            await Task.Delay(Constants.CycleSeconds);
            ApplicationDataManager.SaveData(_state.Data);
        }
    }

    private void sessionTime_Click(object sender, EventArgs e)
    {

    }
}
