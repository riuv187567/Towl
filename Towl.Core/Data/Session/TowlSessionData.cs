namespace Towl.Core.Data.Session;

public struct TowlSessionData()
{
    public Dictionary<string, ProcessEntry> ProcessEntries { get; set; } = [];
}