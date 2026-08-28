namespace Towl.Core.Data;

public class ProcessEntry()
{
    public string Name { get; set; } = "";
    public long TotalSeconds { get; set; } = 0;
    public Dictionary<DateOnly, long> DateEntries { get; set; } = [];
}

public struct TowlSessionData()
{
    public Dictionary<string, ProcessEntry> ProcessEntries { get; set; } = [];
}