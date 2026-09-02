namespace Towl.Core.Data.Settings;

public struct DiscordIntegrationSettings()
{
    public bool EnableDiscordStatus { get; set; } = false;
    public string DiscordAppId { get; set; } = "";
}
