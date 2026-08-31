using DiscordRPC;
using Towl.Core.Data;
using Towl.Core.Utils;

namespace Towl.Core;

public class DiscordIntegration : IDisposable
{
    private readonly TowlState _state;
    private readonly DiscordRpcClient? _client;

    public DiscordIntegration(TowlState state)
    {
        _state = state;

        if (!_state.Settings.DiscordIntegration.EnableDiscordStatus)
            return;

        try
        {
            _client = new DiscordRpcClient(_state.Settings.DiscordIntegration.DiscordAppId);

            _client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Connected to discord with user {0}", e.User.Username);
                Console.WriteLine("Avatar: {0}", e.User.GetAvatarURL(User.AvatarFormat.WebP));
                Console.WriteLine("Decoration: {0}", e.User.GetAvatarDecorationURL());
            };

            _client.OnError += (sender, e) =>
            {
                // Nothing
            };

            _client.Initialize();

            SetDescription($"Tracked Time - {TimeUtils.HumanizeTime(7061760)}");
        }
        catch (Exception)
        {
            _client = null;
        }
    }

    public void SetDescription(string description)
    {
        _client?.SetPresence(new RichPresence()
        {
            Type = ActivityType.Playing,
            Details = description,
            Timestamps = null
        });
    }

    public void Dispose() => _client?.Dispose();
}
