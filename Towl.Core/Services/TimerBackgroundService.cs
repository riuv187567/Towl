using Microsoft.Extensions.Hosting;
using Towl.Core.Data;
using Towl.Core.Data.Session;
using Towl.Core.Utils;

namespace Towl.Core.Services;

public class TimerBackgroundService(TowlState state) : BackgroundService
{
    private readonly TowlState _state = state;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var time = new PeriodicTimer(TimeSpan.FromSeconds(Constants.CycleSeconds));

        while (await time.WaitForNextTickAsync(stoppingToken))
        {
            var today = TimeUtils.GetToday();

            if (!_state.CursorMoved)
                continue;

            foreach (var tracked in _state.Settings.TrackedProcessSettings)
            {
                var foundProcess = System.Diagnostics.Process.GetProcessesByName(tracked.Name).Length != 0;

                if (!foundProcess)
                    continue;

                if (!ProcessUtils.ProcessIsFocused(tracked.Name))
                    continue;

                var processEntry = new ProcessEntry() { Name = tracked.Name, TotalSeconds = 1 };

                if (_state.Data.ProcessEntries.TryGetValue(tracked.Name, out var value))
                    processEntry = value;
                else
                    _state.Data.ProcessEntries.Add(tracked.Name, processEntry);

                if (processEntry.DateEntries.ContainsKey(today))
                    processEntry.DateEntries[today] += 1;
                else
                    processEntry.DateEntries.Add(today, 1);

                processEntry.TotalSeconds = processEntry.DateEntries.Select((pair) => pair.Value).Sum();
            }
        }
    }
}