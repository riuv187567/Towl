using System.Text.Json.Serialization;

namespace Towl.Core.Data.Session;

public class VaultSessionData()
{
    [JsonInclude]
    [JsonPropertyName("ProcessEntries")]
    private Dictionary<string, ProcessEntry> _processEntries = [];

    [JsonIgnore]
    public readonly object Lock = new();

    public void AddSeconds(string processName, DateOnly day, int seconds)
    {
        lock (Lock)
        {
            var processEntry = new ProcessEntry() { Name = processName };

            if (_processEntries.TryGetValue(processName, out var value))
                processEntry = value;
            else
                _processEntries.Add(processName, processEntry);

            if (processEntry.DateEntries.ContainsKey(day))
                processEntry.DateEntries[day] += seconds;
            else
                processEntry.DateEntries.Add(day, seconds);
        }
    }

    public long GetSeconds(string processName, DateOnly day)
    {
        lock (Lock)
        {
            if (!_processEntries.TryGetValue(processName, out ProcessEntry? value))
                return 0;

            if (!value.DateEntries.TryGetValue(day, out long time))
                return 0;

            return time;
        }
    }

    public long GetTotalSeconds(string processName)
    {
        lock (Lock)
        {
            if (!_processEntries.TryGetValue(processName, out ProcessEntry? value))
                return 0;

            return value.TotalSeconds;
        }
    }

    public bool ProcessEntryExist(string processName)
    {
        lock (Lock)
        {
            if (_processEntries.TryGetValue(processName, out ProcessEntry? value))
                return true;

            return false;
        }
    }
}