namespace Towl.Core.Data.Session;

public static class VaultSessionDataExtensions
{
    public static long GetTodaySeconds(this VaultSessionData data, string processName)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        return data.GetSeconds(processName, today);
    }
}
