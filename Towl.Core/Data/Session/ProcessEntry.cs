namespace Towl.Core.Data.Session;

public class ProcessEntry()
{
    public string Name { get; set; } = "";
    public Dictionary<DateOnly, long> DateEntries { get; set; } = [];

    public long TotalSeconds => DateEntries.Values.Sum();
}
