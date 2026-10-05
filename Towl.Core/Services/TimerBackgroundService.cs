using Microsoft.Extensions.Hosting;
using Towl.Core.Data;
using Towl.Core.Utils;

namespace Towl.Core.Services;

public class TimerBackgroundService(VaultManager state, CursorMovedBackgroundService cursorMovedTest) : BackgroundService
{
    private readonly VaultManager _state = state;
    private readonly CursorMovedBackgroundService _cursorMovedTest = cursorMovedTest;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var time = new PeriodicTimer(TimeSpan.FromSeconds(Constants.CycleSeconds));

        while (await time.WaitForNextTickAsync(stoppingToken))
        {
            var today = TimeUtils.GetToday();

            if (!_cursorMovedTest.CursorMoved)
                continue;

            foreach (var tracked in _state.Current!.Settings.TrackedProcessSettings)
            {
                if (!ProcessUtils.ProcessExist(tracked.Name))
                    continue;

                if (!ProcessUtils.ProcessIsFocused(tracked.Name))
                    continue;

                _state.Current!.Data.AddSeconds(tracked.Name, today, Constants.CycleSeconds);
            }
        }
    }
}