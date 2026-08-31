using System.Windows;
using System.Windows.Media;
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
            await UpdateMainTimer();
            var time = new PeriodicTimer(TimeSpan.FromSeconds(Constants.CycleSeconds));

            while (await time.WaitForNextTickAsync())
            {
                await UpdateMainTimer();
                await UpdateSecondaryTimer();
                await UpdateStatusBar();

                await Task.Delay(Constants.CycleSeconds);
                TowlDataManager.SaveData(_state.Data);
            }
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
            _discord.SetDescription($"Tracked Time - {todayTimeString}");
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

            var activeColor = new SolidColorBrush
            {
                Color = Color.FromArgb(Constants.ActiveColor.A, Constants.ActiveColor.R, Constants.ActiveColor.G, Constants.ActiveColor.B)
            };

            var notActiveColor = new SolidColorBrush
            {
                Color = Color.FromArgb(Constants.NotActiveColor.A, Constants.NotActiveColor.R, Constants.NotActiveColor.G, Constants.NotActiveColor.B)
            };

            var notFoundColor = new SolidColorBrush
            {
                Color = Color.FromArgb(Constants.NotFoundColor.A, Constants.NotFoundColor.R, Constants.NotFoundColor.G, Constants.NotFoundColor.B)
            };

            if (!_state.Data.ProcessEntries.TryGetValue(displayedProcName, out var process))
                StatusBar.Fill = notFoundColor;
            else
            {
                if (ProcessUtils.ProcessIsFocused(displayedProcName) && _state.CursorMoved)
                    StatusBar.Fill = activeColor;
                else
                    StatusBar.Fill = notActiveColor;
            }
        }
    }
}