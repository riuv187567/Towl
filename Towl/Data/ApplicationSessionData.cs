namespace Towl.Data;

public class ProcessEntry()
{
    public string Name { get; set; } = "";
    public long TotalSeconds { get; set; } = 0;
    public Dictionary<DateOnly, long> DateEntries { get; set; } = [];
}

public struct ApplicationSessionData()
{
    public Dictionary<string, ProcessEntry> ProcessEntries { get; set; } = [];
}