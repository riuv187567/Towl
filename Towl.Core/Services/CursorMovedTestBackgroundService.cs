using Microsoft.Extensions.Hosting;
using System.Drawing;
using Towl.Core.Data;

namespace Towl.Core.Services;

public class CursorMovedTestBackgroundService(Func<Point> cursorPosition, TowlState state) : BackgroundService
{
    private readonly Func<Point> _cursorPosition = cursorPosition;
    private readonly TowlState _state = state;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var time = new PeriodicTimer(TimeSpan.FromSeconds(Constants.CursorMoveTimeout));
        var cursorPosition = _cursorPosition();

        while (await time.WaitForNextTickAsync(stoppingToken))
        {
            var newCursorPosition = _cursorPosition();

            _state.CursorMoved = newCursorPosition != cursorPosition;
            cursorPosition = newCursorPosition;
        }
    }
}