namespace Towl.Data;

public class TrackedProcessSettings()
{
    public string Name { get; set; } = "";
    public long SpeculativeHours { get; set; } = 0;
}

public struct ApplicationSettings()
{
    public string DisplayedProcessName { get; set; } = "";
    public List<TrackedProcessSettings> TrackedProcessSettings { get; set; } = [];
}
