namespace Towl.Core.Data.Settings;

public struct VaultSettings()
{
    public string DisplayedProcessName { get; set; } = "";
    public List<TrackedProcessSettings> TrackedProcessSettings { get; set; } = [];
    public DiscordIntegrationSettings DiscordIntegration { get; set; } = new();
}
