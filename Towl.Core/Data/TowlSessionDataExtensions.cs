namespace Towl.Core.Data;

public static class TowlSessionDataExtensions
{
    public static long GetTodaySeconds(this TowlSessionData data, string processName)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        if (!data.ProcessEntries.TryGetValue(processName, out ProcessEntry? value))
            return 0;

        if (!value.DateEntries.TryGetValue(today, out long time))
            return 0;

        return time;
    }
}
