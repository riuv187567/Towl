using Towl.Core.Data.Session;
using Towl.Core.Data.Settings;

namespace Towl.Core.Data;

public class Vault
{
    public required VaultSessionData Data { get; set; } = new();
    public required VaultSettings Settings { get; set; } = new();
}
