using DiscordRPC;
using Towl.Utils;

namespace Towl;

public class DiscordIntegration : IDisposable
{
    private readonly string DISCORD_APP_ID = "1539586832474574850";
    private readonly DiscordRpcClient client;

    public DiscordIntegration()
    {
        client = new DiscordRpcClient(DISCORD_APP_ID);

        client.OnReady += (sender, e) =>
        {
            Console.WriteLine("Connected to discord with user {0}", e.User.Username);
            Console.WriteLine("Avatar: {0}", e.User.GetAvatarURL(User.AvatarFormat.WebP));
            Console.WriteLine("Decoration: {0}", e.User.GetAvatarDecorationURL());
        };

        client.Initialize();

        SetDescription($"Tracked Time - {TimeUtils.HumanizeTime(7061760)}");
    }

    public void SetDescription(string description)
    {
        client.SetPresence(new RichPresence()
        {
            Type = ActivityType.Playing,
            Details = description,
            Timestamps = null
        });
    }

    public void Dispose()
    {
        client.Dispose();
    }
}
