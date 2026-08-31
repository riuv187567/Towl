using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Towl.Core.Utils;

public class ProcessUtils
{
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    static extern bool GetCursorPos(out Point lpPoint);

    static public bool ProcessIsFocused(string processName)
    {
        var runninProcesses = Process.GetProcessesByName(processName);
        var activeWindowHandle = GetForegroundWindow();

        foreach (Process process in runninProcesses)
            if (process.MainWindowHandle.Equals(activeWindowHandle))
                return true;

        return false;
    }

    static public Point GetCursorPosition()
    {
        GetCursorPos(out var point);
        return point;
    }
}
