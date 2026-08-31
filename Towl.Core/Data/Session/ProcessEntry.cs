namespace Towl.Core.Data.Session;

public class ProcessEntry()
{
    public string Name { get; set; } = "";
    public long TotalSeconds { get; set; } = 0;
    public Dictionary<DateOnly, long> DateEntries { get; set; } = [];
}
