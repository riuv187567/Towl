using System.Windows;
using Towl.Core;
using Towl.Core.Data;
using Towl.Core.Utils;

namespace Towl.WPF
{
    public partial class TowlWindow : Window
    {
        private readonly TowlState _state;
        private readonly DiscordIntegration _discord;

        public TowlWindow(TowlState state, DiscordIntegration discord)
        {
            InitializeComponent();

            _state = state;
            _discord = discord;

            Loaded += async (a, b) => RunTimerAsync();
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
                // sessionTime.BackColor = Constants.NotActiveColor;
                MainTimeText.Text = Constants.NoProcessDisplayedText;
            }
            else
            {
                /*
                if (ProcessUtils.ProcessIsFocused(displayedProcName) && _state.CursorMoved)
                    MainTimeText.Background = Constants.ActiveColor;
                else
                    sessionTime.BackColor = Constants.NotActiveColor;
                */

                var todayTimeString = TimeUtils.HumanizeTime(_state.Data.GetTodaySeconds(process.Name));
                MainTimeText.Text = todayTimeString;
                _discord.SetDescription($"Tracked Time - {todayTimeString}");
            }
        }
    }
}