using Microsoft.Extensions.Hosting;
using System.Drawing;

namespace Towl.Core.Services;

public class CursorMovedBackgroundService(Func<Point> cursorPosition) : BackgroundService
{
    private readonly Func<Point> _cursorPosition = cursorPosition;
    public bool CursorMoved { get; private set; } = false;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var time = new PeriodicTimer(TimeSpan.FromSeconds(Constants.CursorMoveTimeout));
        var cursorPosition = _cursorPosition();

        while (await time.WaitForNextTickAsync(stoppingToken))
        {
            var newCursorPosition = _cursorPosition();

            CursorMoved = newCursorPosition != cursorPosition;
            cursorPosition = newCursorPosition;
        }
    }
}