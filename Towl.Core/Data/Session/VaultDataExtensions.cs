namespace Towl.Core.Data.Session;

public static class VaultDataExtensions
{
    public static long GetTodaySeconds(this VaultData data, string processName)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        return data.GetSeconds(processName, today);
    }
}
