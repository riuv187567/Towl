using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Towl.Utils;

public class ProcessUtils
{
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    static public bool ProcessIsFocused(string processName)
    {
        var runninProcesses = Process.GetProcessesByName(processName);
        var activeWindowHandle = GetForegroundWindow();

        foreach (Process process in runninProcesses)
            if (process.MainWindowHandle.Equals(activeWindowHandle))
                return true;

        return false;
    }
}
