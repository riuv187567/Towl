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

    static public bool ProcessExist(string processName)
    {
        return Process.GetProcessesByName(processName).Length != 0;
    }

    static public bool ProcessIsFocused(string processName)
    {
        try
        {
            var runninProcesses = Process.GetProcessesByName(processName);
            var activeWindowHandle = GetForegroundWindow();

            foreach (Process process in runninProcesses)
                if (process.MainWindowHandle.Equals(activeWindowHandle))
                    return true;
        }
        catch { }

        return false;
    }

    static public Point GetCursorPosition()
    {
        GetCursorPos(out var point);
        return point;
    }
}
