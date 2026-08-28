using Microsoft.Extensions.Hosting;
using Towl.Data;

namespace Towl.Services;

public class CursorMovedTestBackgroundService(ApplicationState state) : BackgroundService
{
    private readonly ApplicationState _state = state;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var time = new PeriodicTimer(TimeSpan.FromSeconds(Constants.CursorMoveTimeout));
        var cursorPosition = Cursor.Position;

        while (await time.WaitForNextTickAsync(stoppingToken))
        {
            var newCursorPosition = Cursor.Position;

            _state.CursorMoved = newCursorPosition != cursorPosition;
            cursorPosition = newCursorPosition;
        }
    }
}