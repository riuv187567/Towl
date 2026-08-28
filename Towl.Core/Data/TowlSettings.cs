namespace Towl.Core.Data;

public class TrackedProcessSettings()
{
    public string Name { get; set; } = "";
    public long SpeculativeHours { get; set; } = 0;
}

public struct TowlSettings()
{
    public string DisplayedProcessName { get; set; } = "";
    public List<TrackedProcessSettings> TrackedProcessSettings { get; set; } = [];

    public string DiscordAppId { get; set; } = "";
}
